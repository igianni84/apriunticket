﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="crm_advise_it.login1"  %>
<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>--%>
<%--<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">--%>
    
    

     

  <%--<div id="content" >
	<div id="login-area">        
		<span class="image">
            <img src="images/logo-grigio.png" />
            </span>
		<div class="container">
			
			<div class="login" > 
               <asp:Panel ID="Panel2" runat="server" >
                    <h1 class="section-title">Accedi</h1>
                <div class="field">
					<asp:TextBox ID="txtEmailI" runat="server" CssClass="input-text" placeholder="Inserisci la tua mail" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Controllo" ControlToValidate="txtEmailI" Display="Dynamic" EnableClientScript="false" runat="server"  ErrorMessage="*" CssClass ="p"></asp:RequiredFieldValidator>
				</div>
                <div class="field">
					<asp:TextBox ID="txtPswd" runat="server" TextMode="Password" CssClass="input-text" placeholder="Inserisci la tua password" Width="90%"></asp:TextBox>
				    <asp:RequiredFieldValidator ID="controllopsw" ControlToValidate="txtPswd" Display="Dynamic" EnableClientScript="false" runat="server"  ErrorMessage="*" CssClass ="p"></asp:RequiredFieldValidator>
                </div>
               
                <div class="button-area">
					<asp:LinkButton ID="LinkButton1" runat="server" Text="Recupera Password" ></asp:LinkButton>
                </div> 
                <div class="button-area">					
                    <br />
                    <asp:Button ID="Login" runat="server" Text="Login" CssClass ="button black" />&nbsp;&nbsp;&nbsp;<asp:Button ID="reset"
                                        runat="server" Text="Reset" CssClass ="button black" Visible="false" /> <br /><br />
                    <asp:Label ID="Label1" runat="server" Text="Utente non abilitato" Visible="false" ForeColor="Red" CssClass ="order-status waiting"  ></asp:Label>
				</div>
                </asp:Panel>

                

                     <asp:Panel ID="Panel6" runat="server" border="0"  visible="false"  >                        
                        <div class="button-area"> 
                            <asp:Label ID="lblOK" runat="server" Text="Email inviata con successo" CssClass ="order-status executed" ></asp:Label>
                        </div>                       
                        <div class="field">
                            <h7 class="label">Controlla il tuo account di posta</h7>
                        </div>
                        <br />
                        <div class="button-area">
                            <asp:LinkButton ID="LinkButton2" runat="server" Text="Torna al login" ></asp:LinkButton>
                        </div>
                     </asp:Panel>

                     <asp:Panel ID="Panel7" runat="server" visible="false" CssClass ="current"> 
                            <div class="field">
                                <asp:Label ID="Label2" runat="server" Text="Inserisci email comunicata al momento della sottoscrizione"></asp:Label>
                            </div>      
                           <div class="field">
                            <asp:TextBox ID="txtEmailRec" runat="server" CssClass ="input-text" placeholder="Inserisci la tua email" ></asp:TextBox>
                           </div>
                           <div class="button-area">
					            <asp:Button ID="btnInvia" runat="server" Text="RECUPERA" CssClass ="button black" />
                                 <asp:Label ID="lblE" runat="server" Font-Bold ="true" CssClass="order-status waiting" ForeColor ="Red"></asp:Label>
                                <br /><br /><br /><asp:LinkButton ID="LinkButton3" runat="server" Text="Torna al login" ></asp:LinkButton>
                           </div>
                       
                    </asp:Panel>

                   
          </div>
          </div> 
          
        <div class="clear"></div>		
	</div>
</div>--%>
          



          <!DOCTYPE html>
<!-- 
Template Name: Metronic - Responsive Admin Dashboard Template build with Twitter Bootstrap 3.2.0
Version: 3.2.0
Author: KeenThemes
Website: http://www.keenthemes.com/
Contact: support@keenthemes.com
Follow: www.twitter.com/keenthemes
Like: www.facebook.com/keenthemes
Purchase: http://themeforest.net/item/metronic-responsive-admin-dashboard-template/4021469?ref=keenthemes
License: You must have a valid license purchased only from themeforest(the above link) in order to legally use the theme for your project.
-->
<!--[if IE 8]> <html lang="en" class="ie8 no-js"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9 no-js"> <![endif]-->
<!--[if !IE]><!-->
<html>
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
<meta charset="utf-8"/>
<title>ApriUnTicket.it | Login </title>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<meta http-equiv="Content-type" content="text/html; charset=utf-8">
<meta content="" name="description"/>
<meta content="" name="author"/>
<!-- BEGIN GLOBAL MANDATORY STYLES -->
<link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css"/>
<link href="../../assets2/global/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/global/plugins/simple-line-icons/simple-line-icons.min.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/global/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/global/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css" rel="stylesheet" type="text/css"/>
<!-- END GLOBAL MANDATORY STYLES -->
<!-- BEGIN PAGE LEVEL STYLES -->
<link href="../../assets2/global/plugins/select2/select2.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/admin/pages/css/login-soft.css" rel="stylesheet" type="text/css"/>
<!-- END PAGE LEVEL SCRIPTS -->
<!-- BEGIN THEME STYLES -->
<link href="../../assets2/global/css/components.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/global/css/plugins.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/admin/layout/css/layout.css" rel="stylesheet" type="text/css"/>
<link id="style_color" href="../../assets2/admin/layout/css/themes/default.css" rel="stylesheet" type="text/css"/>
<link href="../../assets2/admin/layout/css/custom.css" rel="stylesheet" type="text/css"/>
<!-- END THEME STYLES -->
<link rel="shortcut icon" href="images/favicon.ico"/>
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<!-- DOC: Apply "page-header-fixed-mobile" and "page-footer-fixed-mobile" class to body element to force fixed header or footer in mobile devices -->
<!-- DOC: Apply "page-sidebar-closed" class to the body and "page-sidebar-menu-closed" class to the sidebar menu element to hide the sidebar by default -->
<!-- DOC: Apply "page-sidebar-hide" class to the body to make the sidebar completely hidden on toggle -->
<!-- DOC: Apply "page-sidebar-closed-hide-logo" class to the body element to make the logo hidden on sidebar toggle -->
<!-- DOC: Apply "page-sidebar-hide" class to body element to completely hide the sidebar on sidebar toggle -->
<!-- DOC: Apply "page-sidebar-fixed" class to have fixed sidebar -->
<!-- DOC: Apply "page-footer-fixed" class to the body element to have fixed footer -->
<!-- DOC: Apply "page-sidebar-reversed" class to put the sidebar on the right side -->
<!-- DOC: Apply "page-full-width" class to the body element to have full width page without the sidebar menu -->
<body class="login">

<!-- BEGIN LOGO -->
<div class="logo">
	<a href="login.aspx">
	<img src="images/logobianco.png" alt="" width="200px"/>
	</a>
</div>
<!-- END LOGO -->
<!-- BEGIN SIDEBAR TOGGLER BUTTON -->
<div class="menu-toggler sidebar-toggler">
</div>
<!-- END SIDEBAR TOGGLER BUTTON -->
<!-- BEGIN LOGIN -->
<div class="content" >
	<!-- BEGIN LOGIN FORM -->
	<form  runat="server" >
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Panel ID="Panel2" runat="server" class="login-form" >
		<h3 class="form-title">Login</h3>
        <div class="alert alert-danger display-hide">
			<button class="close" data-close="alert"></button>
			<span>
			Enter any username and password. </span>
		</div>
        <div class="form-group">
			<!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
			<label class="control-label visible-ie8 visible-ie9">Username</label>
			<div class="input-icon">
				<i class="fa fa-user"></i>
				<asp:TextBox class="form-control placeholder-no-fix" ID="txtEmailI"  runat="server" type="text" autocomplete="off" placeholder="Username" name="username"/>
			    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtEmailI" Display="Dynamic" EnableClientScript="false" runat="server"  ErrorMessage="La username è richiesta" ForeColor ="Salmon"  ></asp:RequiredFieldValidator>
               
            </div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9" >Password</label>
			<div class="input-icon">
				<i class="fa fa-lock"></i>
				<asp:TextBox class="form-control placeholder-no-fix" id="txtPswd" runat="server" type="password" autocomplete="off" placeholder="Password" name="password"/>
			    <asp:RequiredFieldValidator ID="controllopsw" ControlToValidate="txtPswd" Display="Dynamic" EnableClientScript="false" runat="server"  ErrorMessage="La password è richiesta" ForeColor ="Salmon" ></asp:RequiredFieldValidator>
                
            </div>
		</div>
        <asp:Label ID="lblE" runat="server" ForeColor ="Salmon" Visible ="false"  >Credenziali Errate</asp:Label>
                               
		<div class="form-actions">
			<label class="checkbox">
			<input type="checkbox" id="cbxRicorda" name="remember" value="1" runat="server" /> Ricorda credenziali </label>
			<asp:Button class="btn blue pull-right" ID="Login" runat="server" Text="Login">
						</asp:Button>
		</div>
		
        <div class="forget-password">
			<h4>Vuoi recuperare la tua password ?</h4>		
				
			<asp:LinkButton ID="LinkButton1" runat="server" Text="Clicca qui" ></asp:LinkButton>
		
		</div>
        <%--<div class="login-options">
			<h4>Social Network</h4>
			<ul class="social-icons">
				<li>
					<a class="facebook" data-original-title="facebook" href="https://www.facebook.com/adviseita" target ="_blank" >
					</a>
				</li>
				<li>
					<a class="twitter" data-original-title="Twitter" href="#">
					</a>
				</li>
				<li>
					<a class="googleplus" data-original-title="Goole Plus" href="https://plus.google.com/+AdviseIta/about" target="_blank" >
					</a>
				</li>
				<li>
					<a class="linkedin" data-original-title="Linkedin" href="https://www.linkedin.com/company/advise-srl" target="_blank" >
					</a>
				</li>
			</ul>
		</div>--%>
		<%--<div class="create-account">
			<p>
				 Don't have an account yet ?&nbsp; <a href="javascript:;" id="register-btn">
				Create an account </a>
			</p>
		</div>--%>
    </asp:Panel> 
	<%--</form>
	<!-- END LOGIN FORM -->
	<!-- BEGIN FORGOT PASSWORD FORM -->
	<form class="forget-form" action="index.html" method="post" runat="server">--%>
    <asp:Panel ID="Panel7" runat="server" class="login-form" Visible ="false" > 
                     
		<h3> Password Dimenticata ?</h3>
		<p>
			 Inserisci il tuo indirizzo email.
		</p>
		<div class="form-group">
			<div class="input-icon">
				<i class="fa fa-envelope"></i>
				<asp:TextBox ID="txtEmailRec" runat="server" class="form-control placeholder-no-fix" type="text" autocomplete="off" placeholder="Email" name="email"/>
			    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtEmailRec" Display="Dynamic" EnableClientScript="false" runat="server"  ErrorMessage="La mail è richiesta" ForeColor ="Salmon"  ></asp:RequiredFieldValidator>
               
            </div>
		</div>
		<div class="form-actions">
			<asp:Button class="btn blue pull-right" ID="btnBack" runat="server" Text="Back">
						</asp:Button>
			<asp:Button ID="btnInvia" runat="server" type="submit" class="btn blue pull-right" Text="Recupera">
			
			</asp:Button>
		</div>
    </asp:Panel> 
    <asp:Panel ID="Panel6" runat="server" class="login-form"  visible="false"  >                        
                       <h3> Mail inviata correttamente</h3>
		                <p>
			                 Controlla il tuo account di posta
		                </p>
                        <div class="form-actions">
                            <asp:LinkButton ID="LinkButton2" runat="server" Text="Torna al login" ></asp:LinkButton>
                        </div>
                     </asp:Panel>
     </ContentTemplate>
    </asp:UpdatePanel>
	</form>
	<!-- END FORGOT PASSWORD FORM -->
	<!-- BEGIN REGISTRATION FORM -->
	<%--<form class="register-form" action="index.html" method="post" runat="server">
		<h3>Sign Up</h3>
		<p>
			 Enter your personal details below:
		</p>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Full Name</label>
			<div class="input-icon">
				<i class="fa fa-font"></i>
				<input class="form-control placeholder-no-fix" type="text" placeholder="Full Name" name="fullname"/>
			</div>
		</div>
		<div class="form-group">
			<!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
			<label class="control-label visible-ie8 visible-ie9">Email</label>
			<div class="input-icon">
				<i class="fa fa-envelope"></i>
				<input class="form-control placeholder-no-fix" type="text" placeholder="Email" name="email"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Address</label>
			<div class="input-icon">
				<i class="fa fa-check"></i>
				<input class="form-control placeholder-no-fix" type="text" placeholder="Address" name="address"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">City/Town</label>
			<div class="input-icon">
				<i class="fa fa-location-arrow"></i>
				<input class="form-control placeholder-no-fix" type="text" placeholder="City/Town" name="city"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Country</label>
			<select name="country" id="select2_sample4" class="select2 form-control">
				<option value=""></option>
				<option value="AF">Afghanistan</option>
				<option value="AL">Albania</option>
				<option value="DZ">Algeria</option>
				<option value="AS">American Samoa</option>
				<option value="AD">Andorra</option>
				<option value="AO">Angola</option>
				<option value="AI">Anguilla</option>
				<option value="AQ">Antarctica</option>
				<option value="AR">Argentina</option>
				<option value="AM">Armenia</option>
				<option value="AW">Aruba</option>
				<option value="AU">Australia</option>
				<option value="AT">Austria</option>
				<option value="AZ">Azerbaijan</option>
				<option value="BS">Bahamas</option>
				<option value="BH">Bahrain</option>
				<option value="BD">Bangladesh</option>
				<option value="BB">Barbados</option>
				<option value="BY">Belarus</option>
				<option value="BE">Belgium</option>
				<option value="BZ">Belize</option>
				<option value="BJ">Benin</option>
				<option value="BM">Bermuda</option>
				<option value="BT">Bhutan</option>
				<option value="BO">Bolivia</option>
				<option value="BA">Bosnia and Herzegowina</option>
				<option value="BW">Botswana</option>
				<option value="BV">Bouvet Island</option>
				<option value="BR">Brazil</option>
				<option value="IO">British Indian Ocean Territory</option>
				<option value="BN">Brunei Darussalam</option>
				<option value="BG">Bulgaria</option>
				<option value="BF">Burkina Faso</option>
				<option value="BI">Burundi</option>
				<option value="KH">Cambodia</option>
				<option value="CM">Cameroon</option>
				<option value="CA">Canada</option>
				<option value="CV">Cape Verde</option>
				<option value="KY">Cayman Islands</option>
				<option value="CF">Central African Republic</option>
				<option value="TD">Chad</option>
				<option value="CL">Chile</option>
				<option value="CN">China</option>
				<option value="CX">Christmas Island</option>
				<option value="CC">Cocos (Keeling) Islands</option>
				<option value="CO">Colombia</option>
				<option value="KM">Comoros</option>
				<option value="CG">Congo</option>
				<option value="CD">Congo, the Democratic Republic of the</option>
				<option value="CK">Cook Islands</option>
				<option value="CR">Costa Rica</option>
				<option value="CI">Cote d'Ivoire</option>
				<option value="HR">Croatia (Hrvatska)</option>
				<option value="CU">Cuba</option>
				<option value="CY">Cyprus</option>
				<option value="CZ">Czech Republic</option>
				<option value="DK">Denmark</option>
				<option value="DJ">Djibouti</option>
				<option value="DM">Dominica</option>
				<option value="DO">Dominican Republic</option>
				<option value="EC">Ecuador</option>
				<option value="EG">Egypt</option>
				<option value="SV">El Salvador</option>
				<option value="GQ">Equatorial Guinea</option>
				<option value="ER">Eritrea</option>
				<option value="EE">Estonia</option>
				<option value="ET">Ethiopia</option>
				<option value="FK">Falkland Islands (Malvinas)</option>
				<option value="FO">Faroe Islands</option>
				<option value="FJ">Fiji</option>
				<option value="FI">Finland</option>
				<option value="FR">France</option>
				<option value="GF">French Guiana</option>
				<option value="PF">French Polynesia</option>
				<option value="TF">French Southern Territories</option>
				<option value="GA">Gabon</option>
				<option value="GM">Gambia</option>
				<option value="GE">Georgia</option>
				<option value="DE">Germany</option>
				<option value="GH">Ghana</option>
				<option value="GI">Gibraltar</option>
				<option value="GR">Greece</option>
				<option value="GL">Greenland</option>
				<option value="GD">Grenada</option>
				<option value="GP">Guadeloupe</option>
				<option value="GU">Guam</option>
				<option value="GT">Guatemala</option>
				<option value="GN">Guinea</option>
				<option value="GW">Guinea-Bissau</option>
				<option value="GY">Guyana</option>
				<option value="HT">Haiti</option>
				<option value="HM">Heard and Mc Donald Islands</option>
				<option value="VA">Holy See (Vatican City State)</option>
				<option value="HN">Honduras</option>
				<option value="HK">Hong Kong</option>
				<option value="HU">Hungary</option>
				<option value="IS">Iceland</option>
				<option value="IN">India</option>
				<option value="ID">Indonesia</option>
				<option value="IR">Iran (Islamic Republic of)</option>
				<option value="IQ">Iraq</option>
				<option value="IE">Ireland</option>
				<option value="IL">Israel</option>
				<option value="IT">Italy</option>
				<option value="JM">Jamaica</option>
				<option value="JP">Japan</option>
				<option value="JO">Jordan</option>
				<option value="KZ">Kazakhstan</option>
				<option value="KE">Kenya</option>
				<option value="KI">Kiribati</option>
				<option value="KP">Korea, Democratic People's Republic of</option>
				<option value="KR">Korea, Republic of</option>
				<option value="KW">Kuwait</option>
				<option value="KG">Kyrgyzstan</option>
				<option value="LA">Lao People's Democratic Republic</option>
				<option value="LV">Latvia</option>
				<option value="LB">Lebanon</option>
				<option value="LS">Lesotho</option>
				<option value="LR">Liberia</option>
				<option value="LY">Libyan Arab Jamahiriya</option>
				<option value="LI">Liechtenstein</option>
				<option value="LT">Lithuania</option>
				<option value="LU">Luxembourg</option>
				<option value="MO">Macau</option>
				<option value="MK">Macedonia, The Former Yugoslav Republic of</option>
				<option value="MG">Madagascar</option>
				<option value="MW">Malawi</option>
				<option value="MY">Malaysia</option>
				<option value="MV">Maldives</option>
				<option value="ML">Mali</option>
				<option value="MT">Malta</option>
				<option value="MH">Marshall Islands</option>
				<option value="MQ">Martinique</option>
				<option value="MR">Mauritania</option>
				<option value="MU">Mauritius</option>
				<option value="YT">Mayotte</option>
				<option value="MX">Mexico</option>
				<option value="FM">Micronesia, Federated States of</option>
				<option value="MD">Moldova, Republic of</option>
				<option value="MC">Monaco</option>
				<option value="MN">Mongolia</option>
				<option value="MS">Montserrat</option>
				<option value="MA">Morocco</option>
				<option value="MZ">Mozambique</option>
				<option value="MM">Myanmar</option>
				<option value="NA">Namibia</option>
				<option value="NR">Nauru</option>
				<option value="NP">Nepal</option>
				<option value="NL">Netherlands</option>
				<option value="AN">Netherlands Antilles</option>
				<option value="NC">New Caledonia</option>
				<option value="NZ">New Zealand</option>
				<option value="NI">Nicaragua</option>
				<option value="NE">Niger</option>
				<option value="NG">Nigeria</option>
				<option value="NU">Niue</option>
				<option value="NF">Norfolk Island</option>
				<option value="MP">Northern Mariana Islands</option>
				<option value="NO">Norway</option>
				<option value="OM">Oman</option>
				<option value="PK">Pakistan</option>
				<option value="PW">Palau</option>
				<option value="PA">Panama</option>
				<option value="PG">Papua New Guinea</option>
				<option value="PY">Paraguay</option>
				<option value="PE">Peru</option>
				<option value="PH">Philippines</option>
				<option value="PN">Pitcairn</option>
				<option value="PL">Poland</option>
				<option value="PT">Portugal</option>
				<option value="PR">Puerto Rico</option>
				<option value="QA">Qatar</option>
				<option value="RE">Reunion</option>
				<option value="RO">Romania</option>
				<option value="RU">Russian Federation</option>
				<option value="RW">Rwanda</option>
				<option value="KN">Saint Kitts and Nevis</option>
				<option value="LC">Saint LUCIA</option>
				<option value="VC">Saint Vincent and the Grenadines</option>
				<option value="WS">Samoa</option>
				<option value="SM">San Marino</option>
				<option value="ST">Sao Tome and Principe</option>
				<option value="SA">Saudi Arabia</option>
				<option value="SN">Senegal</option>
				<option value="SC">Seychelles</option>
				<option value="SL">Sierra Leone</option>
				<option value="SG">Singapore</option>
				<option value="SK">Slovakia (Slovak Republic)</option>
				<option value="SI">Slovenia</option>
				<option value="SB">Solomon Islands</option>
				<option value="SO">Somalia</option>
				<option value="ZA">South Africa</option>
				<option value="GS">South Georgia and the South Sandwich Islands</option>
				<option value="ES">Spain</option>
				<option value="LK">Sri Lanka</option>
				<option value="SH">St. Helena</option>
				<option value="PM">St. Pierre and Miquelon</option>
				<option value="SD">Sudan</option>
				<option value="SR">Suriname</option>
				<option value="SJ">Svalbard and Jan Mayen Islands</option>
				<option value="SZ">Swaziland</option>
				<option value="SE">Sweden</option>
				<option value="CH">Switzerland</option>
				<option value="SY">Syrian Arab Republic</option>
				<option value="TW">Taiwan, Province of China</option>
				<option value="TJ">Tajikistan</option>
				<option value="TZ">Tanzania, United Republic of</option>
				<option value="TH">Thailand</option>
				<option value="TG">Togo</option>
				<option value="TK">Tokelau</option>
				<option value="TO">Tonga</option>
				<option value="TT">Trinidad and Tobago</option>
				<option value="TN">Tunisia</option>
				<option value="TR">Turkey</option>
				<option value="TM">Turkmenistan</option>
				<option value="TC">Turks and Caicos Islands</option>
				<option value="TV">Tuvalu</option>
				<option value="UG">Uganda</option>
				<option value="UA">Ukraine</option>
				<option value="AE">United Arab Emirates</option>
				<option value="GB">United Kingdom</option>
				<option value="US">United States</option>
				<option value="UM">United States Minor Outlying Islands</option>
				<option value="UY">Uruguay</option>
				<option value="UZ">Uzbekistan</option>
				<option value="VU">Vanuatu</option>
				<option value="VE">Venezuela</option>
				<option value="VN">Viet Nam</option>
				<option value="VG">Virgin Islands (British)</option>
				<option value="VI">Virgin Islands (U.S.)</option>
				<option value="WF">Wallis and Futuna Islands</option>
				<option value="EH">Western Sahara</option>
				<option value="YE">Yemen</option>
				<option value="ZM">Zambia</option>
				<option value="ZW">Zimbabwe</option>
			</select>
		</div>
		<p>
			 Enter your account details below:
		</p>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Username</label>
			<div class="input-icon">
				<i class="fa fa-user"></i>
				<input class="form-control placeholder-no-fix" type="text" autocomplete="off" placeholder="Username" name="username"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Password</label>
			<div class="input-icon">
				<i class="fa fa-lock"></i>
				<input class="form-control placeholder-no-fix" type="password" autocomplete="off" id="register_password" placeholder="Password" name="password"/>
			</div>
		</div>
		<div class="form-group">
			<label class="control-label visible-ie8 visible-ie9">Re-type Your Password</label>
			<div class="controls">
				<div class="input-icon">
					<i class="fa fa-check"></i>
					<input class="form-control placeholder-no-fix" type="password" autocomplete="off" placeholder="Re-type Your Password" name="rpassword"/>
				</div>
			</div>
		</div>
		<div class="form-group">
			<label>
			<input type="checkbox" name="tnc"/> I agree to the <a href="#">
			Terms of Service </a>
			and <a href="#">
			Privacy Policy </a>
			</label>
			<div id="register_tnc_error">
			</div>
		</div>
		<div class="form-actions">
			<button id="register-back-btn" type="button" class="btn">
			<i class="m-icon-swapleft"></i> Back </button>
			<button type="submit" id="register-submit-btn" class="btn blue pull-right">
			Sign Up <i class="m-icon-swapright m-icon-white"></i>
			</button>
		</div>
	</form>--%>
	<!-- END REGISTRATION FORM -->
</div>
<!-- END LOGIN -->
<!-- BEGIN COPYRIGHT -->
<div class="copyright">
	 2014 &copy; Advise srl.
</div>
<!-- END COPYRIGHT -->
<!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
<!-- BEGIN CORE PLUGINS -->
<!--[if lt IE 9]>
<script src="../../assets/global/plugins/respond.min.js"></script>
<script src="../../assets/global/plugins/excanvas.min.js"></script> 
<![endif]-->
<script src="../../assets2/global/plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>
<!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->
<script src="../../assets2/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/jquery.blockui.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/jquery.cokie.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js" type="text/javascript"></script>
<!-- END CORE PLUGINS -->
<!-- BEGIN PAGE LEVEL PLUGINS -->
<script src="../../assets2/global/plugins/jquery-validation/js/jquery.validate.min.js" type="text/javascript"></script>
<script src="../../assets2/global/plugins/backstretch/jquery.backstretch.min.js" type="text/javascript"></script>
<script type="text/javascript" src="../../assets2/global/plugins/select2/select2.min.js"></script>
<!-- END PAGE LEVEL PLUGINS -->
<!-- BEGIN PAGE LEVEL SCRIPTS -->
<script src="../../assets2/global/scripts/metronic.js" type="text/javascript"></script>
<script src="../../assets2/admin/layout/scripts/layout.js" type="text/javascript"></script>
<script src="../../assets2/admin/layout/scripts/quick-sidebar.js" type="text/javascript"></script>
<script src="../../assets2/admin/layout/scripts/demo.js" type="text/javascript"></script>
<script src="../../assets2/admin/pages/scripts/login-soft.js" type="text/javascript"></script>
<!-- END PAGE LEVEL SCRIPTS -->
<script>
    jQuery(document).ready(function () {
        Metronic.init(); // init metronic core components
        Layout.init(); // init current layout
        QuickSidebar.init(); // init quick sidebar
        Demo.init(); // init demo features
        Login.init();
        // init background slide images
        $.backstretch([
        "../../assets2/admin/pages/media/bg/1.jpg",
        "../../assets2/admin/pages/media/bg/2.jpg",
        "../../assets2/admin/pages/media/bg/3.jpg",
        "../../assets2/admin/pages/media/bg/4.jpg"
        ], {
            fade: 1000,
            duration: 8000
        }
    );
    });
function back-btn_onclick() {

}

</script>
<!-- END JAVASCRIPTS -->
</body>
<!-- END BODY -->
</html>












				
   
  

<%--   
</asp:Content>--%>
