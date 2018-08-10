using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace VOTKWIK_API.SMS
{
    public class BoSms
    {
        public bool SendSMS(string UserName, string contactNumber, string Message)
        {
            try
            {
                const string accountSid = "ACce70b455c5be1cb084f4421bb454d1f9";
                const string authToken = "af53e78139e6d0baf1df9a9bf6073d65";

                // Initialize the Twilio client
                TwilioClient.Init(accountSid, authToken);

                // make an associative array of people we know, indexed by phone number
                //var people = new Dictionary<string, string>()
                //{
                //    {"+919664344325", "Bhavesh Desai"},
                //    {"+919892807072","Sunita Desai" }
                //};

                var people = new Dictionary<string, string>()
                {
                    {contactNumber, UserName}
                };


                // Iterate over all our friends
                //foreach (var person in people)
                //{
                // Send a new outgoing SMS by POSTing to the Messages resource
                MessageResource.Create
                (
                    from: new PhoneNumber("+1 256-818-3042"), // From number, must be an SMS-enabled Twilio number
                    to: new PhoneNumber(contactNumber), // To number, if using Sandbox see note above
                                                        // Message content
                    body: $"Hey " + UserName + Message
               );
                //    Console.WriteLine($"Sent message to {person.Value}");
                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}