using Newtonsoft.Json;
using NotificationServiceSDK;
string templateId = "7AB69885-1AD2-4A98-8578-EA8EE02938CF";
string apiKey = "api_key";
var client = new Client(apiKey);



var from = new EmailAddress("baorlys.dev@gmail.com", "Bảo Lý");
var tos = new List<string>
            {
                "ankhoa2003@gmail.com"
            };

var option1 = new Dictionary<string, string>
            {
                {"name", "Khoa" }
            };

var dynamicTemplateData = new Dictionary<string, object>
            {
                 { MimeType.Html, option1 }
            };
var mailHelper = new MailHelper();
var message = mailHelper.CreateNotification(
    from,
    tos,
    templateId,
    dynamicTemplateData,
    TargetOutput.EMAIL
    );

var result = await client.SendEmailAsync(message);
Console.WriteLine(result);