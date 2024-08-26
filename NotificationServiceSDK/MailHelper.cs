

namespace NotificationServiceSDK
{
    public class MailHelper
    {
        public Message CreateNotification(EmailAddress from, List<string> tos, string? templateId, Dictionary<string, object> data, TargetOutput target)
        {
            Message message = new Message();
            message.SetFrom(from);
            message.AddTo(tos);
            message.SetTemplateId(templateId);
            message.AddContents(data);
            message.SetTarget(target);
            return message;
        }
    }
}
