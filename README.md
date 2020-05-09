Projet réalisé par Feng Chung, Jeanne Gardebois, Axel Baroux, Thomas Franel
MTI 2021

# Tuto Déploiement
## Requierements
* Visual Studio 2019
* Microsoft sql server management studio
* Gestionnaire des services internet (IIS)
* Le projet BarManagement.tar
* Installer le bundle d’hébergement .NET Core
## Mise en place du déploiement
## 1) IIS
* Lancez IIS et créez un nouveau pool d'application.
* Donnez lui un nom (dans l'exemple: bar.management), indiquez la version du CLR.NET la plus récente (ici : V4.0), et mettez le mode pipeline "intégré".
* Le pool application devrait se lancer automatiquement (vérifiez que son état est bien "démarré").

## 2) MSSMS
* Lancez le MSSMS et créez la database "barDB"
* Lancez le script database.sql pour générer la database.

* Lancez le script suivant pour créer un nouveau login du nom de IIS APPPOOL\bar.management (permet de faire la connexion entre cette base de donnée et le serveur IIS) :

 CREATE LOGIN [IIS APPPOOL\bar.management] FROM WINDOWS;
* Vérifiez manuellement que ce login (situé dans le fichier sécurité du server sql) a bien la base de donnée barDB par défaut. Donnez lui un rôle suffisant pour qu'il puisse intéragir avec la base de donnée. (db_owner par exemple). Vérifiez également les autorisations pour le server (dans l'onglet "Eléments sécurisables").
* Allez dans la database "barDB" et allez jusqu'à l'onglet "sécurité" -> "utilisateurs"
*  Créez un nouvel utilisateur :
nom d'utilisateur (ici): barUser
nom de connexion : cliquez sur les ... et allez chercher IIS APPPOOL
schéma par défaut (ici): db_owner
* Toujours dans la création du user, allez dans l'onglet "schéma appartenant à un rôle"  et "appartenance" et indiquez db_owner.

## 3) VISUAL STUDIO
* Dézipez la tarball et lancer le .sln dans Visual Studio
* Dans le fichier appsettings.json,  remplacez la valeur de "Server" par l'instance SQL contenant votre nouvelle database barDB.
EXEMPLE:
"ConnectionStrings": "BarDB":"Server=DESKTOPO28RHQD\\;Database=barDB;Trusted_Connection=True"
* Remplacez de la même manière la connection string située dans le fichier DataAccess\EfModels\barDBContext.cs.
* Clique-droit sur le projet barManagement -> Publier...
* Dans publier, choisir "Dossier" : indiquez le chemin où vous souhaitez publier le projet (globalement n'importe ou)
* Cliquez sur ...avancé 
		-> options de publications des fichiers -> coché
		-> Bases de données -> utiliser barDB
Enregistrez -> Creéz un profil
* Appuyez sur Publier : vous devez obtenir :
========== Génération : 1 a réussi, 0 a échoué, 0 mis à jour, 0 a été ignoré ==========
========== Publication : 1 a réussi, 0 a échoué, 0 a été ignoré ==========

## Déploiement
## 1) VISUAL STUDIO
* Allez au niveau du chemin que vous avez indiquez pour la publication du projet.
* Ouvrez web.config : remplacer la partie correspondante (```<configuration></configuration>```) par celle-ci: 
```
 <configuration>
  <location path="." inheritInChildApplications="false">
  <appSettings>
    <add key="webpages:Enabled" value="true" />
  </appSettings>
    <system.webServer>
     <modules runAllManagedModulesForAllRequests="true" /> 
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
        <remove name="ExtensionlessUrlHandler-Integrated-4.0" />
        <add name="ExtensionlessUrlHandler-Integrated-4.0" path="*" verb="*" type="System.Web.Handlers.TransferRequestHandler" resourceType="Unspecified" requireAccess="Script" preCondition="integratedMode,runtimeVersionv4.0" />
      </handlers>
      <aspNetCore processPath="C:\Program Files\dotnet\dotnet.exe" arguments=".\BarManagement.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" />
    </system.webServer>
  </location>
</configuration>
```
* Copiez ce dossier et collez-le -> %SystemDrive%\inetpub\wwwroot
* Allez dans les propriétés de ce dossier -> onglet sécurités -> Ajoutez le groupe utilisateur "tout le monde" et donnez lui tous les droits. (N'oubliez pas d'appliquer).

## IIS
* Créez ensuite un nouveau site web (clique droit sur "Sites").
nom du site : bar.management
chemin d'accès: chemin menant vers %SystemDrive%\inetpub\wwwroot
nom de l'hôte: bar.management
* Déroulez le nouveau bar.management : vous devez voir votre dossier. clic droit -> convertir en application...
* Une fois votre application créée, vous pouvez la lancer : le site est maintenant déployé.

## Besoin d'aide ?
### Si le déploiement ne s'est pas passé comme prévu
- Reprend chaque étape une à une et vérifie que tu n'as rien oublié.
- Utilise l'outils: Observateurs d'événements disponible sur windows. Journaux Windows -> Application => séries de logs qui t'aideront à mieux comprendre les erreurs.
### Vérifie
- panneau de configuration -> Programmes -> Activer/Désactiver des fonctionnalités windows
Coche : 
	 - Internet Informations Services -> Services World Wild Web -> ASPNET 4.7 
	 - Internet Informations Services -> Services World Wild Web -> CGI