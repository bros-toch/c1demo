<?xml version="1.0"  encoding="UTF-8" ?>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyUrlAction.aspx.cs" Inherits="AttachingCustomElementsSample.MyUrlAction" %>

<%@ Register TagPrefix="control"  TagName="httpheaders"  Src="~/Composite/controls/HttpHeadersControl.ascx" %>
<%@ Register TagPrefix="control"  TagName="scriptloader"  Src="~/Composite/controls/ScriptLoaderControl.ascx" %>
<%@ Register TagPrefix="control"  TagName="styleloader"  Src="~/Composite/controls/StyleLoaderControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml"  xmlns:ui="http://www.w3.org/1999/xhtml"  xmlns:control="http://www.composite.net/ns/uicontrol">
<control:httpheaders ID="Httpheaders1"  runat="server" />
    

<head>
    <title>My URL Action</title>
    <control:styleloader ID="Styleloader1"  runat="server" />
    <control:scriptloader ID="Scriptloader1"  type="sub"  runat="server" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script type="text/javascript">
        var setFocus = function (a) {
            a = $(a);

            var token = a.data('token');

            EventBroadcaster.broadcast(BroadcastMessages.SYSTEMTREEBINDING_FOCUS, token);
        };

        var executeAction = function (a) {
            a = $(a);

            var providerName = a.data('providername');
            var entityToken = a.data('entitytoken');
            var actionToken = a.data('actiontoken');
            var piggybag = a.data('piggybag');
            var piggybagHash = a.data('piggybaghash');

            var clientElement = new ClientElement(providerName, entityToken, piggybag, piggybagHash);
            var actionElement = new ActionElement(a.html(), actionToken);

            var systemAction = new SystemAction(actionElement);
            var systemNode = new SystemNode(clientElement);

            SystemAction.invoke(systemAction, systemNode);
        };

        function ClientElement(providerName, entityToken, piggybag, piggybagHash) {
            this.ProviderName = providerName;
            this.EntityToken = entityToken;
            this.Piggybag = piggybag;
            this.PiggybagHash = piggybagHash;
            this.HasChildren = false;
            this.IsDisabled = false;
            this.DetailedDropSupported = false;
            this.ContainsTaggedActions = false;
            this.TreeLockEnabled = false;

            return this;
        };

        function ActionElement(label, actionToken) {
            this.Label = label;
            this.ActionToken = actionToken;

            return this;
        };

        function edit() {
            var providerName = '<%: providerName %>';
            var entityToken = '<%: entityToken %>';
            var actionToken = "actionTokenType='Composite\\.C1Console\\.Workflow\\.WorkflowActionToken,Composite'actionToken='_WorkflowType_=\\'Composite\\\\\\.Plugins\\\\\\.Elements\\\\\\.ElementProviders\\\\\\.PageElementProvider\\\\\\.EditPageWorkflow,Composite\\\\\\.Workflows\\'_Payload_=\\'\\'_ExtraPayload_=\\'\\'_Ignore_=\\'False\\'_PermissionTypes_=\\'\\''actionTokenHash='-1974740862'";// "<%= actionToken %>";
            var piggybag = '<%: piggybag %>';
            var piggybagHash = '<%: piggybagHash %>';

            var clientElement = new ClientElement(providerName, entityToken, piggybag, piggybagHash);
            var actionElement = new ActionElement($('#edit').html(), actionToken);

            var systemAction = new SystemAction(actionElement);
            var systemNode = new SystemNode(clientElement);

            SystemAction.invoke(systemAction, systemNode);
        }
        console.log(edit.toString())
        window.edit = edit;
    </script>
</head>
<body>
    <ui:dialogpage label="My URL Action"  image="${skin}/dialogpages/message16.png">
        <ui:scrollbox>
            <asp:PlaceHolder ID="MyPlaceHolder"  runat="server" />
            <a id="edit" href="#" onclick="edit()">Edit</a>
        </ui:scrollbox>
    </ui:dialogpage>
</body>
</html>
