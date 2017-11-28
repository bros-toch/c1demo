using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using java.util;
using Syn.WordNet;

namespace Demo.EBook
{
    public static class NplHelper
    {
        private static readonly string[] STOPWORDS = System.IO.File.ReadAllLines(HostingEnvironment.MapPath("~/App_Data/EBook/Stopwords/english.txt")).Select(x => x.ToLower()).Distinct().ToArray();

        private static readonly string[] BAD_WORDS = new[] {"adverbs.txt", "conjunctions.txt", "others.txt"}.SelectMany(x => File.ReadAllLines(Path.Combine(HostingEnvironment.MapPath("~/App_Data/EBook/Words"), x)))
            .Select(x => x.ToLower())
            .ToArray();

        private static WordNetEngine WordNet = new WordNetEngine();


        public static IEnumerable<string> FindWords(string text)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.4-models.jar`
            var jarRoot = @"stanford-corenlp-3.4-models\";

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            props.setProperty("sutime.binders", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically 
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // these are all the sentences in this document
            // a CoreMap is essentially a Map that uses class objects as keys and has values with custom types
            var sentences = annotation.get(typeof(CoreAnnotations.SentencesAnnotation));
            if (sentences == null)
            {
                return Enumerable.Empty<string>();
            }

            var words = new List<string>();
            foreach (Annotation sentence in sentences as ArrayList)
            {
                words.Add(sentence.toString());
            }

            return words;
        }

        public static IEnumerable<WordCount> FindVocabularies(string text)
        {
            var allWordQuery = from x in text.Split().ToList()
                group x by x.Trim('“', '”', ',', '.', '?', '—', '’', '‘', '(', ')', '[', ']', '!').Trim().ToLower()
                into G
                orderby G.Key
                let count = G.Count()
                select new WordCount { Word = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(G.Key), OccurrenceCount = count };

            var allWords = allWordQuery
                .Where(x => !STOPWORDS.Contains(x.Word.ToLower()))
                .Where(x=> !BAD_WORDS.Contains(x.Word.ToLower()))
                .Where(x =>
                {
                    long num;
                    if (long.TryParse(x.Word, out num))
                    {
                        return false;
                    }
                    return true;
                })
                .ToList();

            return allWords;
        }

        public static List<SynSet> FindDefinition(string word)
        {
            if (!WordNet.IsLoaded)
            {
                WordNet.LoadFromDirectory(HostingEnvironment.MapPath("~/App_Data/EBook/Wordnet"));
            }
            return WordNet.GetSynSets(word);
        }
        
    }
}
