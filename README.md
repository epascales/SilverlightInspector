# SilverlightInspector
[![PayPayl donate button](https://www.paypalobjects.com/en_AU/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=YXQS7UVKWJ3B2&lc=US&item_name=Support%20our%20open%2dsource%20initiatives&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted"Donate once-off to this project using Paypal")

##Installation

var shell = Container.Resolve<ShellView>();

Application.Current.RootVisual = shell;

//insert this line after root visual initialization (usually i App.cs file)

SilverlightInspector.Inspector.Run();
