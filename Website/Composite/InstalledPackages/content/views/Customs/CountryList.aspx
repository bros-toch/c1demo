<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CountryList.aspx.cs"
    Inherits="CountryList" %>

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:control="http://www.composite.net/ns/uicontrol" xmlns:ui="http://www.w3.org/1999/xhtml">
<control:httpheaders runat="server" />
<head id="Head1" runat="server">
    <control:styleloader runat="server" />
    <control:scriptloader type="sub" runat="server" />
    <title>Data Store Migrator</title>
    <link type="text/css" rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css"/>
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


    </script>
    
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <style type="text/css">
        html,body { overflow: scroll; }
        
    </style>
</head>
<body>
    <form id="mainForm" runat="server">
            <div class="padded container well">
                <asp:Repeater ID="rptCountryList" runat="server" OnItemDataBound="rptCountryList_OnItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-strip">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="ltLink" runat="server" Mode="PassThrough" />

                                <%--<button type="button" class="edit btn btn-primary btn-xs" 
                                    data-providername="<%# Eval("Id") %>"
                                        data-entitytoken="<%# Eval("Id") %>"
                                        data-actiontoken="<%# Eval("Id") %>"
                                        data-piggybag="<%# Eval("Id") %>"
                                        data-piggybaghash="<%# Eval("Id") %>"

                                    >Edit</button>--%>
                            </td>
                            <td><%# Eval("Label") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        
        
        <%--<a href="#" data-providername='DynamicSqlDataProvider' data-entitytoken='{"Type":"Demo.Country","Source":"63ec1a73-b1ed-4ec8-923f-2840448c43ce","Id":"eec4eac6-a4e4-4315-800f-000b3f7631fa","meta:type":"Consoles.EntityTokens.PageTeaserInstanceEntityToken, App_Code.ntbqddif","meta:hash":"302709324"}' data-actiontoken='actionTokenType='Composite\.C1Console\.Workflow\.WorkflowActionToken,Composite'actionToken='_WorkflowType_=\'AspNetType:Consoles\\\.EditWorkflowAttribute\'_Payload_=\'\'_ExtraPayload_=\'\'_Ignore_=\'False\'_PermissionTypes_=\'\''actionTokenHash='-641263679'' data-piggybag='' data-piggybaghash='479468608' onclick="executeAction(this)">edit</a>--%>

                                
                            
    </form>
    <script type="text/javascript">
        $(function () {
            $('.edit').on('click', function () {

                executeAction(this);
            })
        })
    </script>
</body>
</html>
