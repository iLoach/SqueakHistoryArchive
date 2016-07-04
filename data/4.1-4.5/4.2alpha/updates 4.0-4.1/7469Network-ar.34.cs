"Change Set:		7469Network-ar.34Network-ar.34:UIManagerization. Replaces all the trivial references to PopUpMenu, SelectionMenu, CustomMenu, and FillInTheBlank."!!ServerDirectory methodsFor: 'updates' stamp: 'ar 8/6/2009 19:08'!updateInstallVersion: newVersion	"For each server group, ask whether we want to put the new version marker (eg 'Squeak2.3') at the end of the file.  Current version of Squeak must be the old one when this is done.		ServerDirectory new updateInstallVersion: 'Squeak9.9test'"	| myServers updateStrm names choice indexPrefix listContents version versIndex |	[names := ServerDirectory groupNames asSortedArray.	choice := UIManager default chooseFrom: names values: names.	choice == nil]		whileFalse:		[indexPrefix := (choice endsWith: '*') 			ifTrue: [(choice findTokens: ' ') first]	"special for internal updates"			ifFalse: ['']. 	"normal"		myServers := (ServerDirectory serverInGroupNamed: choice)						checkServersWithPrefix: indexPrefix						andParseListInto: [:x | listContents := x].		myServers size = 0 ifTrue: [^ self].		version := SystemVersion current version.		versIndex := (listContents collect: [:pair | pair first]) indexOf: version.		versIndex = 0 ifTrue:			[^ self inform: 'There is no section in updates.list for your version'].  "abort"		"Append new version to updates following my version"		listContents := listContents copyReplaceFrom: versIndex+1 to: versIndex with: {{newVersion. {}}}.		updateStrm := ReadStream on:			(String streamContents: [:s | Utilities writeList: listContents toStream: s]).		myServers do:			[:aServer | updateStrm reset.			aServer putFile: updateStrm named: indexPrefix ,'updates.list'.			Transcript cr; show: indexPrefix ,'updates.list written on server ', aServer moniker].		self closeGroup]! !!HttpUrl methodsFor: 'downloading' stamp: 'ar 8/6/2009 20:47'!askNamePassword	"Authorization is required by the host site.  Ask the user for a userName and password.  Encode them and store under this realm.  Return false if the user wants to give up."	| user pass |	(self confirm: 'Host ', self toText, 'wants a different user and password.  Type them now?' orCancel: [false])		ifFalse: [^ false].	user := UIManager default request: 'User account name?' initialAnswer: ''.	pass := UIManager default requestPassword: 'Password?'.	Passwords at: realm put: (Authorizer new encode: user password: pass).	^ true! !