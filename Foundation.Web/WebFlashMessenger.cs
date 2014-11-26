using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web.Mvc;
using Foundation.Configuration;
using Foundation.Infrastructure.Notifications;
using Foundation.Web.Extensions;

namespace Foundation.Web
{
    public class WebFlashMessenger : IFlashMessenger
    {
        private readonly Dictionary<FlashMessageType, Queue<string>> messages;
        private readonly ResourceManager resourceManager;
        private readonly Guid uniqueGuid;

        public WebFlashMessenger(IResourcesLocator resourcesLocator)
        {
            resourceManager = resourcesLocator.FlashMessagesResourceManager;

            uniqueGuid = Guid.NewGuid();
            messages = new Dictionary<FlashMessageType, Queue<string>>();

            foreach (FlashMessageType flashMessageType in Enum.GetValues(typeof (FlashMessageType)))
            {
                messages[flashMessageType] = new Queue<string>();
            }
        }

        public void AddMessageByKey(string resourceKey, FlashMessageType messageType)
        {
            if (!string.IsNullOrWhiteSpace(resourceKey))
            {
                string message = resourceManager.GetString(resourceKey);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    string messageToDisplay = message;
                    AddMessage(messageToDisplay, messageType);
                }
            }
        }

        public void AddMessage(string message, FlashMessageType messageType)
        {
            messages[messageType].Enqueue(message);
        }

        public string RenderFlashMessages()
        {
            var sb = new StringBuilder();

            if (HasMessages())
            {
                foreach (object messageType in Enum.GetValues(typeof (FlashMessageType)))
                {
                    string message = RenderFlashMessagesForType((FlashMessageType) messageType);

                    if (message != null)
                    {
                        sb.Append(message);
                    }
                }
            }

            return sb.ToString();
        }

        public string RenderFlashMessagesForType(FlashMessageType messageType)
        {
            IEnumerable<string> messagesToRender = GetMessagesForType(messageType);

            if (messagesToRender.Any())
            {
                var builder = new TagBuilder("div");
                builder.AddCssClass("alert alert-" + EnumExtensions.GetEnumDescription(messageType).ToLower());
                builder.InnerHtml = string.Join(".", messagesToRender);
                return builder.ToString(TagRenderMode.Normal);
            }


            return null;
        }

        public IEnumerable<string> GetMessagesForType(FlashMessageType messageType)
        {
            return messages[messageType].AsEnumerable();
        }

        public bool HasMessages()
        {
            return messages.Any(item => messages[item.Key].Any());
        }
    }
}