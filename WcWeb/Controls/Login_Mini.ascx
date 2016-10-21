<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login_Mini.ascx.cs" EnableViewState="false" Inherits="WillCallWeb.Controls.Login_Mini" %>
<div id="loginsmall">
    <%if (this.Page.ToString().ToLower() != "asp.register_aspx" && this.Page.ToString().ToLower() != "asp.accountupdate_aspx")
      {%>
        <div class="logininfo">
              <%if (this.Page.User.Identity.IsAuthenticated)
              {%>
               <div class="login-title"><%=this.Page.User.Identity.Name %></div>
               <%} %>
               
               <div class="storecredit">
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Always">
                        <ContentTemplate><%if(this.Page.User.Identity.IsAuthenticated && Ctx.Cart.StoreCreditCurrentlyAvailableForProfile > 0) {%>
                            <%=Ctx.Cart.StoreCreditCurrentlyAvailableForProfile.ToString("c")%> store credit
                            <%} else { %>&nbsp;<%} %>        
                        </ContentTemplate>
                    </asp:UpdatePanel>
               </div>
           </div>
           <div class="functions">
           <%if (this.Page.User.Identity.IsAuthenticated)
          {%>
               <div ><a href="/EditProfile.aspx" id="linkEdit" name="linkEdit">edit profile</a></div>
               <div ><asp:LinkButton ID="linkLogout" class="logout-link" runat="server" Text="logout"  CausesValidation="false"
                       onclick="linkLogout_Click" /></div>
          <%}else{%>
            <div><asp:LinkButton ID="linkLogin" CausesValidation="false" runat="server" onclick="linkRegister_Click" CssClass="loginsmall"><span><b>login</b></span></asp:LinkButton></div>
            <div><a href="/PasswordRecovery.aspx" id="linkRecover" >forgot password</a></div>
            <div><asp:LinkButton ID="linkCreate" CausesValidation="false" runat="server" Text="create account" onclick="linkRegister_Click" /></div>
         <%}%>
         </div>
     <% }%>
</div>