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
// using play.billing.v3;

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

        // Connection to the Google Play billing service.
        //private InAppBillingServiceConnection mBillingConnection;

        // The list of products available for this app.
        //private IList<Product> mProducts;
        // NO BILLING YET

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


        

        protected void onStart()
        {
            base.OnStart();


            //System.Console.WriteLine("Is connecting..");
            //mGooglePlayClient.Connect();
        }

        protected void onStop()
        {
            base.OnStop();

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

        public void ConnectP()
        {
            mGooglePlayClient.Connect();
        }
    }
}

