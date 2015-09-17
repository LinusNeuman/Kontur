using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using Android.Widget;
// test for google play services
using Android.Media;
using Android.Gms;
using Android.Gms.Analytics;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Games.LeaderBoard;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Gms.Common.Apis;
using Android.Content;
using Android.Views;
// test for in app billing

using Facebook;
using FacebookMonoDroid;
using Xamarin.InAppBilling;
using System.Collections.Generic;

namespace NeonShooter
{
    [Activity(Label = "Kontur"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleTask
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
   // [IntentFilter(new[] { Intent.ActionView },
   //     Categories = new[] { Intent.CategoryLauncher, "my.custom.category" }
   //     , DataHost = "kontur", DataScheme ="bice" )]
    public class Activity1
        : Microsoft.Xna.Framework.AndroidGameActivity
        , IGoogleApiClientConnectionCallbacks
        , IGoogleApiClientOnConnectionFailedListener
        , AudioManager.IOnAudioFocusChangeListener
        

        
    {

        public static GoogleAnalytics analytics;
        public static Tracker tracker;
        

        // Aribitrary numbers just used for identifying requests to the Google services.
        public static int REQUEST_CODE_RESOLVE_ERR = 9000;
        public static int REQUEST_LEADERBOARD = 9001;
        public static int REQUEST_ACHIEVEMENTS = 9002;

        // The main interface for GooglePlay services.
        private IGoogleApiClient mGooglePlayClient;
        private IInAppBillingHandler mAppBilling;
        private InAppBillingServiceConnection _serviceConnection;

        // Connection to the Google Play billing service.
        //private InAppBillingServiceConnection mBillingConnection;

        // The list of products available for this app.
        //private IList<Product> mProducts;
        // NO BILLING YET

        private IList<Product> _products;




        // Tracks what happened last time we tried to log in (this session).
        private ConnectionResult mConnectionResult;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

         

            System.Console.WriteLine("started builder");
            // Create the interface used to interact with Google Play.
            //mGooglePlayClient = new GamesClient.Builder(this, this, this)
            //    .SetGravityForPopups((int)(GravityFlags.Bottom | GravityFlags.CenterHorizontal))
            //    .Create();


         

            analytics = GoogleAnalytics.GetInstance(this);
            analytics.SetLocalDispatchPeriod(1800);

            tracker = analytics.NewTracker("UA-39772266-5");
            tracker.EnableExceptionReporting(true);
            tracker.EnableAdvertisingIdCollection(true);
            tracker.EnableAutoActivityTracking(true);

           
            

            GoogleApiClientBuilder builder = new GoogleApiClientBuilder(this)
                .AddApi(GamesClass.Api)
                .AddScope(GamesClass.ScopeGames)
                .AddApi(PlusClass.Api)
                .AddScope(PlusClass.ScopePlusLogin)
                .AddConnectionCallbacks((IGoogleApiClientConnectionCallbacks)this)
                .AddOnConnectionFailedListener(this)
                .SetGravityForPopups((int)(GravityFlags.Bottom | GravityFlags.CenterHorizontal))
                
                //.SetAccountName("Pelle")
                ;
                //.Build();

            

            

        //builder.SetViewForPopups(getApiClient(), getWindow().getDecorView().findViewById(android.R.id.content));


        mGooglePlayClient = builder.Build();
            //mGooglePlayClient.Connect();

            

            

            //builder.SetViewForPopups(view);
            //pGooglePlayClient.RegisterConnectionCallbacks (this);
            //pGooglePlayClient.IsConnectionFailedListenerRegistered (this);;

            GameRoot.Activity = this;
            var g = new GameRoot();
            SetContentView(g.Window);
            g.Run();
        }



        protected void OnPause()
        {

            base.OnPause();
        }

        protected void OnResume()
        {
            
            base.OnResume();
        }

        protected async void GetProducts()
        {
            _products = await _serviceConnection.BillingHandler.QueryInventoryAsync(new List<string> {
                    "all_ships_unlocked",
                    ReservedTestProductIDs.Purchased
                }, ItemType.Product);
            // Were any products returned?
            if (_products == null)
            {
                // No, abort
                return;
            }
        }



        protected void onStart()
        {
            base.OnStart();


            //System.Console.WriteLine("Is connecting..");
            //mGooglePlayClient.Connect();
        }

        protected void onStop()
        {
            base.OnStop();

            // Are we attached to the Google Play Service?
            if (_serviceConnection != null)
            {
                // Yes, disconnect
                _serviceConnection.Disconnect();
            }

            mGooglePlayClient.Disconnect();
        }

        /// <param name="requestCode">The integer request code originally supplied to
        ///  startActivityForResult(), allowing you to identify who this
        ///  result came from.</param>
        /// <param name="resultCode">The integer result code returned by the child activity
        ///  through its setResult().</param>
        /// <param name="data">An Intent, which can return result data to the caller
        ///  (various data can be attached to Intent "extras").</param>
        /// <summary>
        /// Called when an activity you launched exits, giving you the requestCode
        ///  you started it with, the resultCode it returned, and any additional
        ///  data from it.
        /// </summary>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Check if this was the error code we supplied. That is the only one we care about.
            if (requestCode == REQUEST_CODE_RESOLVE_ERR && resultCode == Result.Ok)
            {
                mGooglePlayClient.Connect();
                return;
            }

            if (_serviceConnection != null)
            {
                if (_serviceConnection.Connected)
                {
                    // Ask the open service connection's billing handler to process this request
                    _serviceConnection.BillingHandler.HandleActivityResult(requestCode, resultCode, data);
                }
            }

            // TODO: Use a call back to update the purchased items
            // or listen to the OnProductPurchased event to
            // handle a successful purchase
        }

        public void OnConnected(Bundle bundle) 
        {
            System.Console.WriteLine("Connected-ish");

        }

        public void OnConnectionFailed(ConnectionResult connectionResult) 
        {
            if(connectionResult.HasResolution)
            {
                try
                {
                    connectionResult.StartResolutionForResult(this, REQUEST_CODE_RESOLVE_ERR);
                }
                catch (Android.Content.IntentSender.SendIntentException e)
                {
                    mGooglePlayClient.Connect();
                }
            }

            mConnectionResult = connectionResult;

            System.Console.WriteLine("Connection failed");
        }

        public void OnConnectionSuspended(int i)
        {

        }

        public void OnAudioFocusChange(AudioFocus af)
        {

        }

        public IGoogleApiClient pGooglePlayClient
        {
            get
            {
                return mGooglePlayClient;
            }
        }

        public void BuyProduct()
        {
            _serviceConnection.BillingHandler.BuyProduct(_products[1]);
        }

        public void ConnectP()
        {
            mGooglePlayClient.Connect();
        }

        public void ConnectIAB()
        {
            // Create a new connection to the Google Play Service
            _serviceConnection = new InAppBillingServiceConnection(this, "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAiwyZ6z / lfEOfbb4m0cmzyo42t0fJx83zE4r1YwgfmHk227AehgtzmsKfmITtyrUhOgWvbkYR2ExW0QsNJN67dQLr + dSLpwDUU9Dxs1qhFDQM3DVxB60L / PLXXQKKed + Q6s6orsnfTA1bxSNNlJdlam + ZuhBaBqLa9LL9kR2CdjF10fWxZCUjQClxCYcPRcrvFfbnbxS0pyXY3lNmEHxtir63iqpKVZXfFowX9S4jVjAc8uYSUeqKPF5rAE0mG5xPrIlF / TcEI4PVh3hTzV7bfHKMdHHUyh1qWV7hin / gN9O7cqWa / Cfm6t2mI + IAsjT0wB5VpuwFGXRzcuzSCc / A7wIDAQAB");
            _serviceConnection.OnConnected += () => {
                // Load available products and any purchases
                GetProducts();
            };

            _serviceConnection.Connect();

           // if(_serviceConnection.Connected)
           // GetProducts();
        }
    }
}

