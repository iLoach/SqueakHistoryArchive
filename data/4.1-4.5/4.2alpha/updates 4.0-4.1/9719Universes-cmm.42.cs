"Change Set:		9719Universes-cmm.42Universes-cmm.42:- Improved guard of authorInitialsPerSe."!!UUniverseEditor methodsFor: 'package editing' stamp: 'cmm 3/9/2010 15:15'!createNewPackage	| package |	package := UPackage new.	Utilities authorInitialsPerSe isEmptyOrNil ifFalse: [		package maintainer: Utilities authorInitialsPerSe	].	^package! !