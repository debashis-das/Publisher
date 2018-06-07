using System;
using System.Collections.Generic;
using Amazon;
using Amazon.Runtime;
using CorporateBulkWork.Registration.DTO;
using JustSaying;
using JustSaying.AwsTools;
using TTL.ManagementInformationService.Client.Contract;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            //need to give aws cred
            var tempCred = new BasicAWSCredentials("", "");
            CreateMeABus.DefaultClientFactory = () =>
                new DefaultAwsClientFactory(tempCred);
            var loggerFactory = new Log4NetProvider();

            var publisher = CreateMeABus.WithLogging(loggerFactory)
                 .InRegion(RegionEndpoint.EUWest1.SystemName)
                 .WithSnsMessagePublisher<BusinessUserRegistrationDetails>();
            for (int i = 0; i < 10; i++)
            {
                var bussinessData = new BusinessUserRegistrationDetails()
                {
                    MasterGuid = new Guid("312916C1-9168-E811-822F-CC3D82DF33E1"),
                    CustomerId = 0,
                    ManagedGroupId = "15",
                    CorporateId = "91371",
                    EntryPointId = "203",
                    AffliatedId = "",
                    CorporateReference = "TBDEMO1",
                    EmailAddress = "demo"+i+"123@test.com",
                    Password = "Password",
                    Title = "MR",
                    ForeName = "ForeName",
                    Surname = "Surname",
                    PhoneNumber = "123456789",
                    AlternatePhoneNumber = "1234567",
                    DataProtectionAct1984ConsentIndicator = "N",
                    DataProtectionAct2003ConsentIndicator = "N",
                    TermsAndConditionsConsentIndicator = true,
                    EmailConfirmationRequired = "Yes",
                    ContactAddressLine1 = "address1",
                    ContactAddressLine2 = "address2",
                    ContactAddressLine3 = "address",
                    ContactAddressLine4 = "address4",
                    ContactAddressLine5 = "address5",
                    ContactPostCode = "SE17EF",
                    ContactCountryCode = "Z0",
                    ProfileAnswers = new List<SubmittedAnswer>()
                    {
                        new SubmittedAnswer() {QuestionText = "Profile Question 1", Text = "Q4*6319"},
                        new SubmittedAnswer() {QuestionText = "Q9", Text = "9"}
                    }

                };
                publisher.Publish(bussinessData);
            }
        }
    }
}
