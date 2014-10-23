<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Prova2.aspx.vb" Inherits="crm_advise_it.Prova2" MasterPageFile ="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


	<head>

		<meta charset="utf-8">

		<title>Kontext: A context-shift transition inspired by iOS</title>

		

		 

	</head>
	<body>

		<article class="kontext">
			<div class="layer one show">
				<h2>Kontext</h2>
				

				
			</div>
			<div class="layer two">
			  <h2>Layer Two</h2>
			</div>
			<div class="layer three">
			  <h2>Layer Three</h2>
			</div>
            <div class="layer four">
			  <h2>Layer Four</h2>
			</div>
		</article>

		<ul class="bullets"></ul>

		
		<script src="js/kontext.js"  type ="text/javascript" ></script>
		<script>
		    // Create a new instance of kontext
		    var k = kontext(document.querySelector('.kontext'));


		    // API METHODS:

		    // k.prev(); // Show prev layer
		    // k.next(); // Show next layer
		    // k.show( 3 ); // Show specific layer
		    // k.getIndex(); // Index of current layer
		    // k.getTotal(); // Total number of layers


		    // DEMO-SPECIFIC:

		    var bulletsContainer = document.body.querySelector('.bullets');

		    // Create one bullet per layer
		    for (var i = 0, len = k.getTotal(); i < len; i++) {
		        var bullet = document.createElement('li');
		        bullet.className = i === 0 ? 'active' : '';
		        bullet.setAttribute('index', i);
		        bullet.onclick = function (event) { k.show(event.target.getAttribute('index')) };
		        bullet.ontouchstart = function (event) { k.show(event.target.getAttribute('index')) };
		        bulletsContainer.appendChild(bullet);
		    }

		    // Update the bullets when the layer changes
		    k.changed.add(function (layer, index) {
		        var bullets = document.body.querySelectorAll('.bullets li');
		        for (var i = 0, len = bullets.length; i < len; i++) {
		            bullets[i].className = i === index ? 'active' : '';
		        }
		    });

		    document.addEventListener('keyup', function (event) {
		        if (event.keyCode === 37) k.prev();
		        if (event.keyCode === 39) k.next();
		    }, false);

		    var touchX = 0;
		    var touchConsumed = false;

		    document.addEventListener('touchstart', function (event) {
		        touchConsumed = false;
		        lastX = event.touches[0].clientX;
		    }, false);

		    document.addEventListener('touchmove', function (event) {
		        event.preventDefault();

		        if (!touchConsumed) {
		            if (event.touches[0].clientX > lastX + 10) {
		                k.prev();
		                touchConsumed = true;
		            }
		            else if (event.touches[0].clientX < lastX - 10) {
		                k.next();
		                touchConsumed = true;
		            }
		        }
		    }, false);

		</script>

		<script>		    !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = "http://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } } (document, "script", "twitter-wjs");</script>

	</body>
 </asp:Content>