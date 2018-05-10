using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.UserActivities;
using Windows.Storage;
using Windows.UI.Shell;

namespace JskyUwpLibs
{
    public sealed class UserActivitiesHelper
    {
        UserActivitySession currentActivity;

        public async void GenerateActivityAsync(string websitePage, string title, string content, string backgroundPath)
        {
            // Get the default UserActivityChannel and query it for our UserActivity. If the activity doesn't exist, one is created.
            UserActivityChannel channel = UserActivityChannel.GetDefault();
            UserActivity userActivity = await channel.GetOrCreateUserActivityAsync("MainPage");

            var uri = new Uri("moepicture://" + title + "?action=view");
            // Populate required properties
            userActivity.VisualElements.DisplayText = "MoePicture";
            userActivity.ActivationUri = uri;

            var folder = Package.Current.InstalledLocation;
            var file = await(await folder.GetFolderAsync("JskyUwpLibs")).GetFileAsync("Activities.json");
            string cardJsonText = await FileIO.ReadTextAsync(file);
            dynamic card = JObject.Parse(cardJsonText);
            card.backgroundImage = backgroundPath;
            card.body[0].items[0].text = title;
            card.body[0].items[1].text = content;
            cardJsonText = JsonConvert.SerializeObject(card);
            // where jsonCardText is a JSON string that represents the card
            userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(cardJsonText);

            //Save
            await userActivity.SaveAsync(); //save the new metadata

            // Dispose of any current UserActivitySession, and create a new one.
            currentActivity?.Dispose();
            currentActivity = userActivity.CreateSession();
        }
    }
}
