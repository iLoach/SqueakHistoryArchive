'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!!StandardScriptingSystem class methodsFor: 'class initialization' stamp: 'mir 11/25/2004 19:01'!removeUnreferencedPlayers	"Remove existing but unreferenced player references"	"StandardScriptingSystem removeUnreferencedPlayers"	References keys do: 		[:key | (References at: key) costume pasteUpMorph			ifNil: [References removeKey: key]].! !PreferenceBrowser removeSelector: #representsSameBrowseeAs:!