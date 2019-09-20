﻿using UnityEngine;
using System.Collections;

public class PopulateScreenFeed : MonoBehaviour
{
    [SerializeField] SocialMediaScenatioMessageFeedType messageFeedType = SocialMediaScenatioMessageFeedType.Default;
    [SerializeField] SocialMediaScenarioTextType[] messages;

    private void Start()
    {
        var screenMessageFeed = GetComponent<ScreenMessageFeedView>();

        screenMessageFeed.Populate(0, SocialMediaScenarioPicker.Instance.CurrentScenario.GetMessageFeed(messageFeedType));

        if (messages != null)
        {
            foreach (var message in messages)
            {
                screenMessageFeed.DisplayMessage(new Message()
                {
                    message = SocialMediaScenarioPicker.Instance.CurrentScenario.GetText(message),
                    profile = SocialMediaScenarioPicker.Instance.CurrentScenario.GetProfile(message)
                });
            }
        }
    }


}
