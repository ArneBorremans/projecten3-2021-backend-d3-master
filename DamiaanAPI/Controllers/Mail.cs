using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using DamiaanAPI.Models;

public class Mail
{
    //TODO tweetaligheid toevoegen
    public static void RegisterMail(Loper loper)
    {

        // Onderwerp
        string subject = "Damiaanexperience - U ben Succesvol geregistreerd - Vous êtes inscrit avec succès";

        // Inhoud van de mail Nederlands
        #region Inhoud mail
        StringBuilder messageBody = new StringBuilder();
        messageBody.Append("Deze mail is een bevesting van u registratie op Damiaanexperience. </br>");
        messageBody.Append("</br>");
        messageBody.Append("Hieronder ziet u een overzicht van uw registratie: </br>");
        messageBody.Append("<p style=\"margin-left:1.5em\">");  // tab voorkeuren start  
        messageBody.Append($"   Voornaam: {loper.FirstName}</br>");
        messageBody.Append($"   Achternaam: {loper.LastName}</br>");
        messageBody.Append($"   Email: {loper.Email}</br>");
        messageBody.Append($"   Gemeente: {loper.Gemeente}");
        messageBody.Append("</p>");  // tab end
        #endregion

        // Inhoud van de mail Frans
        #region Inhoud mail
        StringBuilder messageBody_frans = new StringBuilder();
        messageBody_frans.Append("Ce mail est une confirmation de votre inscription sur Damiaanexperience. </br>");
        messageBody_frans.Append("</br>");
        messageBody_frans.Append("Vous trouverez ci-dessous un aperçu de votre inscription : </ br>");
        messageBody_frans.Append("<p style=\"margin-left:1.5em\">");  // tab voorkeuren start  
        messageBody_frans.Append($"   Prénom: {loper.FirstName}</br>");
        messageBody_frans.Append($"   Nom de famille: {loper.LastName}</br>");
        messageBody_frans.Append($"   Courriel: {loper.Email}</br>");
        messageBody_frans.Append($"   Municipalité: {loper.Gemeente}");
        messageBody_frans.Append("</p>");  // tab end
        #endregion

        VerzendMail(new MailAddress(loper.Email), subject, messageBody, messageBody_frans);
    }

    public static void IngeschrevenRouteMail(RouteLoper routeLoper)
    {

        // Onderwerp
        string subject = "Damiaanexperience - Succesvol ingeschreven voor " + routeLoper.Route.Naam["nl"].ToString();

        // Inhoud van de mail Nederlands
        #region Inhoud mail
        StringBuilder messageBody = new StringBuilder();
        messageBody.Append($"Beste, </br>");
        messageBody.Append("</br>");
        messageBody.Append($"Deze mail is een bevesting van uw inscrhijving voor een wandeltocht.");

        
        messageBody.Append("</br>");
        messageBody.Append("U kan deze zichtbaarheid wijzigen op de website bij 'mijn profiel'.</br>");
        messageBody.Append("</br></br>");

        // links moet nog aangepast worden
        messageBody.Append("Alle informatie over deze route en alle andere wandeltochten is beschikbaar op onze website: ");
        messageBody.Append("<a href='https://angularfrontendd3static.z22.web.core.windows.net/home'>Damiaanexperience</a>");
        messageBody.Append("</br>");
        messageBody.Append("Download ook onze app in de playstore!: ");
        messageBody.Append("<a href='https://play.google.com/store/apps?hl=nl&gl=US'>Damiaanactie app</a>");
        #endregion

        // Inhoud van de mail Frans
        #region Inhoud mail
        StringBuilder messageBody_frans = new StringBuilder();
        messageBody_frans.Append($"Cher, </br>");
        messageBody_frans.Append("</br>");
        messageBody_frans.Append($"Ce courrier est une confirmation de votre inscription à la randonnée. ");

        // Voorkeuren 
        messageBody_frans.Append("Vous trouverez ci-dessous un aperçu des préférences que vous avez choisies : </br>");
        messageBody_frans.Append("<p style=\"margin-left:1.5em\">");  // tab start  

        
        messageBody_frans.Append("</br>");
        messageBody_frans.Append("Vous pouvez modifier cette visibilité sur le site web sous 'mon profil'.</br>");
        messageBody_frans.Append("</br></br>");

        messageBody_frans.Append("Toutes les informations concernant cet itinéraire et toutes les autres visites à pied sont disponibles sur notre site web: ");
        messageBody_frans.Append("<a href='https://angularfrontendd3static.z22.web.core.windows.net/home'>Damiaanexperience</a>");
        messageBody_frans.Append("</br>");
        messageBody_frans.Append("Téléchargez également notre application sur le playstore!: ");
        messageBody_frans.Append("<a href='https://play.google.com/store/apps?hl=nl&gl=US'>Damiaanexperience l'app</a>");
        #endregion

        VerzendMail(new MailAddress(routeLoper.Loper.Email), subject, messageBody, messageBody_frans);

    }

    public static void VoltooideRouteMail(RouteLoper routeLoper)
    {
        // Onderwerp
        string subject = $"Damiaanexperience - Route voltooid door { routeLoper.Loper.FirstName} { routeLoper.Loper.LastName}";

        // Inhoud van de mail Nederlands
        #region Inhoud mail
        StringBuilder messageBody = new StringBuilder();
        messageBody.Append($"Proficiat u hebt {routeLoper.Route.Naam["nl"].ToString()} volledig uitgewandeld!</br>");
        messageBody.Append("---Diploma---</br>");

        // Diploma - nog te vervangen door een echt exemplaar
        LinkedResource diploma = new LinkedResource("./Images/dummy-deelname-diploma.jpg", MediaTypeNames.Image.Jpeg);
        diploma.ContentId = "diploma";
        messageBody.Append("</br></br>");
        messageBody.Append("<img src=cid:diploma>");
        #endregion

        // Inhoud van de mail Frans
        #region Inhoud mail
        StringBuilder messageBody_frans = new StringBuilder();
        messageBody.Append($"Félicitations, vous êtes complètement sorti {routeLoper.Route.Naam["nl"].ToString()}!</br>");
        messageBody.Append("---Diploma---</br>");
        messageBody.Append("<img src=cid:diploma>");

        // Diploma - nog te vervangen door een echt exemplaar
        LinkedResource diploma_frans = new LinkedResource("./Images/dummy-deelname-diploma.jpg", MediaTypeNames.Image.Jpeg);
        diploma.ContentId = "diploma_frans";
        messageBody.Append("</br></br>");
        messageBody.Append("<img src=cid:diploma_frans>");
        #endregion

        VerzendMail(new MailAddress(routeLoper.Loper.Email), subject, messageBody, messageBody_frans, new LinkedResource[] { diploma });

    }

    private static void VerzendMail(MailAddress MailTo, string subject, StringBuilder inhoud_nederlands, StringBuilder inhoud_frans, LinkedResource[] resources = null)
    {
        string userName = "damiaand3@telenet.be";
        string password = "Azurewerktniet1";

        // Mail afkomstig van
        MailAddress mailFrom = new MailAddress(userName,
                   "damiaan experience",
                Encoding.UTF8);

        // Client aanmaken
        // code in brackets needed if authentication required
        SmtpClient client = new SmtpClient("smtp.telenet.be", 587)
        {
            Credentials = new NetworkCredential(userName, password),
            EnableSsl = true
        };

        #region Groeten
        // groeten Nederlands
        inhoud_nederlands.Append("</br></br>");
        inhoud_nederlands.Append("Met vriendelijke groeten,</br>");
        inhoud_nederlands.Append("Damiaan experience - team</br>");

        // groeten Frans
        inhoud_frans.Append("</br></br>");
        inhoud_frans.Append("Bien à vous");
        inhoud_frans.Append("Damiaan experience - team</br>");
        #endregion

        #region Banner toevoegen
        // resource banner  
        LinkedResource banner = new LinkedResource("./Images/voorlopig-email-banner.jpg", MediaTypeNames.Image.Jpeg);
        banner.ContentId = "banner";
//        inhoud_nederlands.Append("</br></br>");
//        inhoud_nederlands.Append("<img src=cid:banner>");

        inhoud_frans.Append("</br></br>");
        inhoud_frans.Append("<img src=cid:banner>");
        #endregion

        // Nederlands en Frans samenvoegen
        StringBuilder inhoud = inhoud_nederlands;        
        inhoud.Append("</br><hr></br>"); // Lijn
        inhoud.Append(inhoud_frans);

        // alle inhoud in een alternate view stoppen
        AlternateView inhoudView = AlternateView.CreateAlternateViewFromString(inhoud.ToString(), Encoding.UTF8, MediaTypeNames.Text.Html);

        // alle linkeResources aan view toevoegen
        List<LinkedResource> resourcesList = resources == null? new List<LinkedResource>() : new List<LinkedResource>(resources);
        resourcesList.Add(banner);
        resourcesList.ForEach(r => inhoudView.LinkedResources.Add(r));

        // MailMessage object bouwen
        MailMessage message = new MailMessage(mailFrom, MailTo);
        message.AlternateViews.Add(inhoudView);
        message.IsBodyHtml = true;
        message.BodyEncoding = Encoding.UTF8;
        message.Subject = subject;
        message.SubjectEncoding = Encoding.UTF8;

        // Verzenden
        try
        {
            client.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}