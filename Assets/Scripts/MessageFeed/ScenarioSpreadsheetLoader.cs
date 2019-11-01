using System;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Drawing;

public static class ScenarioSpreadsheetLoader
{


    public static SocialMediaScenario LoadSocialMediaScenario(string file)
    {
        var result = new SocialMediaScenario();

        result.twitterWithFriends = new MessageFeed();
        result.twitterWithFriends.messages = new List<Message>();

        result.fourChan = new MessageFeed();
        result.fourChan.messages = new List<Message>();

        result.hatespeechMessageFeed = new MessageFeed();
        result.hatespeechMessageFeed.messages = new List<Message>();

        result.pileOnMessageFeed = new MessageFeed();
        result.pileOnMessageFeed.messages = new List<Message>();

        result.senderTwitterFeed = new MessageFeed();
        result.senderTwitterFeed.messages = new List<Message>();

        result.thirdSceneSMSFeed = new MessageFeed();
        result.firstSceneSMSFeed = new MessageFeed();


        using (ExcelPackage p = new ExcelPackage(new FileInfo(file)))
        {
            // Receiver, scene 1
            LoadReceiverTwitter(p.Workbook.Worksheets["ReceiverTwitter"], result);
            LoadReceiverFlow(p.Workbook.Worksheets["ReceiverFlow"], result);
            result.firstSceneSMSFeed.messages = LoadSMSFeed(p.Workbook.Worksheets["First Notification Feed"]);


            // Sender, scene 2
            LoadSenderTwitter(p.Workbook.Worksheets["SenderTwitter"], result);
            Load4Chan(p.Workbook.Worksheets["Sender4Chan"], result);

            if (result.senderTwitterFeed.messages.Count > 0)
            {
                result.senderTwitterFeed.messages[result.senderTwitterFeed.messages.Count - 1].startOfSubMessages = true;
            }

            if (result.fourChan.messages.Count > 0)
            {
                result.fourChan.messages[result.fourChan.messages.Count - 1].pauseHere = true;
            }

            int startOfEnticement = result.senderTwitterFeed.messages.Count;

            LoadSenderTwitter(p.Workbook.Worksheets["SenderEnticement"], result);

            for (int i = startOfEnticement; i < result.senderTwitterFeed.messages.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(result.senderTwitterFeed.messages[i].retweetedBy))
                    continue;

                result.senderTwitterFeed.messages[i].skip = true;
            }

            if (result.senderTwitterFeed.messages.Count > 0)
            {
                result.senderTwitterFeed.messages[result.senderTwitterFeed.messages.Count - 1].pauseHere = true;
            }

            Load4Chan(p.Workbook.Worksheets["SenderEnticement"], result, 6); // Sender Enticement is Twitter with 4Chan to the side
            LoadSenderFlow(p.Workbook.Worksheets["SenderFlow"], result);

            result.recommendedVideos = LoadRecommendedVideos(p.Workbook.Worksheets["Sender Videos"]).ToArray();
            LoadSenderURLS(p.Workbook.Worksheets["Sender Videos"], result);

            // Receiver, scene 3
            result.thirdSceneSMSFeed.messages = LoadSMSFeed(p.Workbook.Worksheets["Second Notification Feed"]);
            LoadPileOnTwitter(p.Workbook.Worksheets["TwitterPileOn"], result);
        }

        return result;
    }

    private static void LoadReceiverTwitter(ExcelWorksheet sheet, SocialMediaScenario scenario)
    {
        scenario.twitterWithFriends.messages.AddRange(LoadNormalTwitterFeed(sheet));
    }

    private static void LoadSenderTwitter(ExcelWorksheet sheet, SocialMediaScenario scenario)
    {
        scenario.senderTwitterFeed.messages.AddRange(LoadNormalTwitterFeed(sheet));
    }

    private static void LoadPileOnTwitter(ExcelWorksheet sheet, SocialMediaScenario scenario)
    {
        scenario.pileOnMessageFeed.messages.AddRange(LoadNormalTwitterFeed(sheet));
    }

    private static void LoadReceiverFlow(ExcelWorksheet sheet, SocialMediaScenario scenario)
    {
        var messages = LoadNormalTwitterFeed(sheet, processMessage: false);

        for (int i = 0; i < messages.Count; i++)
        {
            var messageObject = messages[i];

            if (i == 0)
            {
                messageObject.flash = true;
                messageObject.hateSpeechSubMessage = true;
                messageObject.message = ProcessString(messageObject.message);
                scenario.twitterWithFriends.messages.Add(messageObject);
            }
            else if (i == 1)
            {
                messageObject.message = ProcessString(messageObject.message);
                scenario.hatespeechMessageFeed.messages.Add(messageObject);
            }
            else if (i == 2)
            {
                scenario.receiverProfile = messageObject.profile;
                scenario.receiverMessage = messageObject.message;

                break;
            }
        }
    }

    private static void LoadSenderFlow(ExcelWorksheet sheet, SocialMediaScenario scenario)
    {
        var messages = LoadNormalTwitterFeed(sheet, processMessage: false);

        for (int i = 0; i < messages.Count; i++)
        {
            var messageObject = messages[i];

            if (i == 0)
            {
                messageObject.message = ProcessString(messageObject.message);
                messageObject.senderSubMessage = true;
                scenario.senderTwitterFeed.messages.Add(messageObject);
            }
            else if (i == 1)
            {
                scenario.senderProfile = messageObject.profile;
                scenario.senderMessage = messageObject.message;

                break;
            }
        }
    }

    private static void Load4Chan(ExcelWorksheet sheet, SocialMediaScenario scenario, int offset = 0)
    {
        int rows = sheet.Dimension.Rows;

        int messageColumn = 1 + offset;
        int imageColumn = 2 + offset;

        for (int rowIndex = 1; rowIndex <= rows; rowIndex++)
        {
            var row = sheet.Row(rowIndex);

            // Skip instructions which are coloured
            var colour = row.Style.Fill.BackgroundColor;
            if (colour.Rgb != null)
                continue;

            // Skip header rows
            if (sheet.Cells[rowIndex, messageColumn].Style.Font.Bold && !string.IsNullOrWhiteSpace(sheet.GetValue<string>(rowIndex, messageColumn)))
                continue;

            var message = sheet.GetValue<string>(rowIndex, messageColumn);
            bool useReceiverMessage = sheet.GetValue<string>(rowIndex, imageColumn) == "Receiver Message";

            Sprite image = null;
            var animatedImage = useReceiverMessage ? null : GetAnimatedImage(sheet, rowIndex, imageColumn);

            if (!useReceiverMessage && animatedImage == null)
            {
                image = GetImage(sheet, rowIndex, imageColumn);
            }

            if (!string.IsNullOrWhiteSpace(message) || image || animatedImage)
            {
                scenario.fourChan.messages.Add(new Message
                {
                    message = message,
                    animatedImage = animatedImage,
                    image = image,
                    senderSubMessage = useReceiverMessage,
                    highlight = useReceiverMessage
                });
            }
        }
    }

    private static void LoadSenderURLS(ExcelWorksheet sheet, SocialMediaScenario scenario)
    {
        if (sheet == null)
            return;

        int rows = sheet.Dimension.Rows;

        const int nameColumn = 1;
        const int urlColumn = 2;
        const int startColumn = 3;
        const int titleColumn = 4;

        for (int rowIndex = 1; rowIndex <= rows; rowIndex++)
        {
            var row = sheet.Row(rowIndex);

            // Skip instructions which are coloured
            var colour = row.Style.Fill.BackgroundColor;
            if (colour.Rgb != null)
                continue;


            var name = sheet.GetValue<string>(rowIndex, nameColumn);

            if (name == null)
                continue;

            var url = sheet.GetValue<string>(rowIndex, urlColumn);
            int startTime = 0;

            try
            {
                startTime = sheet.GetValue<int>(rowIndex, startColumn);
            }
            catch 
            {

            }

            switch (name.ToLower())
            {
                case "streamed video":

                    scenario.senderStreamingSkipTime = startTime;
                    scenario.senderStreamingURL = url;

                    break;

                case "youtube video":

                    scenario.senderYoutubeURL = url;
                    scenario.senderYoutubeVideoTitle = sheet.GetValue<string>(rowIndex, titleColumn);

                    break;
            }
        }
    }

    private static List<RecommendedVideo> LoadRecommendedVideos(ExcelWorksheet sheet)
    {
        List<RecommendedVideo> result = new List<RecommendedVideo>();

        if (sheet == null)
            return result;

        int rows = sheet.Dimension.Rows;

        const int titleColumn = 1;
        const int fromColumn = 2;
        const int viewsColumn = 3;
        const int imageColumn = 4;

        for (int rowIndex = 1; rowIndex <= rows; rowIndex++)
        {
            var row = sheet.Row(rowIndex);

            // Skip instructions which are coloured
            var colour = row.Style.Fill.BackgroundColor;
            if (colour.Rgb != null)
                continue;

            var image = GetImage(sheet, rowIndex, imageColumn);

            if (image == null)
                continue;

            result.Add(new RecommendedVideo()
            {
                from = sheet.GetValue<string>(rowIndex, fromColumn),
                title = sheet.GetValue<string>(rowIndex, titleColumn),
                picture = image,
                views = sheet.GetValue<string>(rowIndex, viewsColumn),
            });
        }

        return result;
    }

    private static List<Message> LoadNormalTwitterFeed(ExcelWorksheet sheet, bool processMessage = true)
    {
        List<Message> result = new List<Message>();

        int rows = sheet.Dimension.Rows;

        const int profilePictureColumn = 1;
        const int usernameColumn = 2;
        const int fromTagColumn = 3;
        const int messageColumn = 4;
        const int imageColumn = 5;
        const int retweetedByColumn = 6;

        for (int rowIndex = 1; rowIndex <= rows; rowIndex++)
        {
            var row = sheet.Row(rowIndex);

            // Skip instructions which are coloured
            var colour = row.Style.Fill.BackgroundColor;
            if (colour.Rgb != null)
                continue;

            // Skip header rows
            if (sheet.Cells[rowIndex, profilePictureColumn].Style.Font.Bold && !string.IsNullOrWhiteSpace(sheet.GetValue<string>(rowIndex, profilePictureColumn)))
                continue;

            var profilePicture = GetImage(sheet, rowIndex, profilePictureColumn);
            var username = sheet.GetValue<string>(rowIndex, usernameColumn);
            var tag = sheet.GetValue<string>(rowIndex, fromTagColumn);
            var message = sheet.GetValue<string>(rowIndex, messageColumn);
            var retweetedBy = sheet.GetValue<string>(rowIndex, retweetedByColumn);


            Sprite image = null;
            var animatedImage = GetAnimatedImage(sheet, rowIndex, imageColumn);

            if (animatedImage == null)
            {
                image = GetImage(sheet, rowIndex, imageColumn);
            }

            if (!string.IsNullOrWhiteSpace(message) || image || animatedImage)
            {
                if (processMessage)
                {
                    message = ProcessString(message);
                }

                result.Add(new Message
                {
                    message = message,
                    animatedImage = animatedImage,
                    image = image,
                    profile = new OnlineProfile()
                    {
                        picture = profilePicture,
                        tag = CleanString(tag),
                        username = CleanString(username)
                    },
                    retweetedBy = retweetedBy
                });
            }
        }

        return result;
    }

    private static List<Message> LoadSMSFeed(ExcelWorksheet sheet)
    {
        List<Message> result = new List<Message>();
        int rows = sheet.Dimension.Rows;

        const int fromColumn = 1;
        const int applicationColumn = 2;
        const int messageColumn = 3;

        for (int rowIndex = 1; rowIndex <= rows; rowIndex++)
        {
            var row = sheet.Row(rowIndex);

            // Skip instructions which are coloured
            var colour = row.Style.Fill.BackgroundColor;
            if (colour.Rgb != null)
                continue;

            // Skip header rows
            if (sheet.Cells[rowIndex, fromColumn].Style.Font.Bold)
                continue;

            var from = sheet.GetValue<string>(rowIndex, fromColumn);
            var application = sheet.GetValue<string>(rowIndex, applicationColumn);
            var message = sheet.GetValue<string>(rowIndex, messageColumn);

            if (!string.IsNullOrWhiteSpace(message))
            {
                if (application == null || !int.TryParse(application, out _))
                {
                    application = "";
                }
                else
                {
                    application = "[" + application + "]";
                }

                result.Add(new Message
                {
                    message = application + ProcessString(message),
                    profile = new OnlineProfile()
                    {
                        username = CleanString(from)
                    }
                });
            }
        }

        return result;
    }

    private static AnimatedImage GetAnimatedImage(ExcelWorksheet sheet, int row, int column)
    {
        foreach (var drawing in sheet.Drawings)
        {
            var picture = drawing as ExcelPicture;

            if (picture == null)
                continue;

            // Images are 0 based for row/col yet getting the text is 1 based!!
            if (picture.From.Column + 1 == column && picture.From.Row + 1 == row)
            {
                AnimatedGif gif;

                try
                {
                    gif = new AnimatedGif(picture.Image);
                }
                catch (Exception)
                {
                    return null;
                }

                if (gif.Images.Count > 1)
                {
                    var result = new AnimatedImage();

                    result.images = new Sprite[gif.Images.Count];

                    for (int i = 0; i < result.images.Length; i++)
                    {
                        Sprite sprite;

                        using (var stream = new MemoryStream())
                        {
                            gif.Images[i].Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                            stream.Position = 0;

                            var buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);

                            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                            texture.LoadImage(buffer);

                            var rect = new Rect(0, 0, texture.width, texture.height);

                            sprite = Sprite.Create(texture, rect, rect.size / 2);
                        }

                        result.images[i] = sprite;
                    }

                    result.frameTime = gif.Images[0].Duration / 100.0f;
                    return result;
                }

                break;
            }
        }

        return null;
    }

    private static Sprite GetImage(ExcelWorksheet sheet, int row, int column)
    {
        foreach (var drawing in sheet.Drawings)
        {
            var picture = drawing as ExcelPicture;

            if (picture == null)
                continue;

            // Images are 0 based for row/col yet getting the text is 1 based!!
            if (picture.From.Column + 1 == column && picture.From.Row + 1 == row)
            {
                Sprite sprite;

                using (var stream = new MemoryStream())
                {
                    picture.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    stream.Position = 0;

                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    texture.LoadImage(buffer);

                    var rect = new Rect(0, 0, texture.width, texture.height);

                    sprite = Sprite.Create(texture, rect, rect.size / 2);
                }
                return sprite;
            }
        }

        return null;
    }

    /// <summary>
    /// Colours up hash tags and swaps emojis
    /// </summary>
    private static string ProcessString(string text)
    {
        if (text == null)
            return text;

        var words = text.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i];

            if (word.StartsWith("#") || word.StartsWith("@"))
            {
                words[i] = "<color=#4692da>" + word + "</color>";
            }
            else if (word.StartsWith(":") && word.EndsWith(":"))
            {
                words[i] = "<sprite name=" + word.Substring(1, word.Length - 2) + ">";
            }
        }

        return string.Join(" ", words);
    }

    private static string CleanString(string text)
    {
        if (text == null)
            return text;

        if (text.StartsWith("'"))
        {
            text = text.Substring(1);
        }

        return text.Trim();
    }

}

