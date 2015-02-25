using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using Android.Widget;
// test for google play services
using Android.Media;
using Android.Gms;
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
    [Activity(Label = "NeonShooter"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1
        : Microsoft.Xna.Framework.AndroidGameActivity
        , IGoogleApiClientConnectionCallbacks
        , IGoogleApiClientOnConnectionFailedListener
        , AudioManager.IOnAudioFocusChangeListener
    {

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

            // Create the interface used to interact with Google Play.
            //mGooglePlayClient = new GamesClient.Builder(this, this, this)
            //    .SetGravityForPopups((int)(GravityFlags.Bottom | GravityFlags.CenterHorizontal))
            //    .Create();
            

            GoogleApiClientBuilder builder = new GoogleApiClientBuilder(this)
                .AddApi(GamesClass.Api)
                .AddScope(GamesClass.ScopeGames)
                .AddApi(PlusClass.Api)
                .AddScope(PlusClass.ScopePlusLogin)
                .AddConnectionCallbacks((IGoogleApiClientConnectionCallbacks) this)
                .AddOnConnectionFailedListener(this)
                .SetAccountName("Pelle")
                ;
                //.Build();

            mGooglePlayClient = builder.Build();

            //pGooglePlayClient.RegisterConnectionCallbacks (this);
            //pGooglePlayClient.IsConnectionFailedListenerRegistered (this);

            Toast.MakeText(this, "test", ToastLength.Long);

            GameRoot.Activity = this;
            var g = new GameRoot();
            SetContentView(g.Window);
            g.Run();
        }


        /// <summary>
        /// Helper function for attempting to Log into Google Play servers.
        /// </summary>
        public void LoginToGoogle()
        {
            // If we are already connected/connecting, no need to try again.
            if (mGooglePlayClient.IsConnected || mGooglePlayClient.IsConnecting)
                return;

            // If we haven't already tried to login, do so now. Other wise, try to resolve
            // the issue from the last login attempt.
            if (mConnectionResult == null)
            {
                mGooglePlayClient.Connect();
            }
            else
            {
                ResolveLogin(mConnectionResult);
            }
        }

        private void ResolveLogin(ConnectionResult result)
        {
            // Does this failure reason have a solution?
            if (result.HasResolution)
            {
                try
                {
                    // Try to resolve the problem automatically.
                    result.StartResolutionForResult(this, REQUEST_CODE_RESOLVE_ERR);
                }
                catch (Android.Content.IntentSender.SendIntentException /*e*/)
                {
                    // Not really sure why this is here.
                    mGooglePlayClient.Connect();
                }
            }
        }

        protected void onStart() // no idea what this does but in google's tutorial
        {
            base.OnStart();


            // when is this function being called??? google i want answers
            mGooglePlayClient.Connect(); // connects the client? i dont know
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
            Toast.MakeText(this, "Connected", ToastLength.Long).Show();

        }

        public void OnConnectionFailed(ConnectionResult connectionResult) 
        {
            ResolveLogin(connectionResult);

            mConnectionResult = connectionResult;

            Toast.MakeText(this, "Connection failed", ToastLength.Long).Show();
        }

        public void OnConnectionSuspended(int i)
        {

        }

        public void OnAudioFocusChange(AudioFocus af)
        {

        }
    }
}

