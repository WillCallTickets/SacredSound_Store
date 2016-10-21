<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Banner.ascx.cs" Inherits="WillCallWeb.Controls.Banner" %>
<asp:Panel ID="pnlDisplay" runat="server" class="banner-division">
    <%if (this._bannerFilePath.Trim().Length > 0)
      {%>
        <div class="bannerimage">
            <%=this._startUrl %>
            <img src='<%=this._bannerFilePath %>' />
            <%=this._endUrl %>
        </div>
    <%}
      else
      { %>
        <div class="bannertext">
            <%=this._promotext.Trim() %>
            <%=this._textUrl %>
        </div>
    <%} %>
</asp:Panel>

    