"Change Set:		9488EToys-ar.61EToys-ar.61:Move Player cleanUp from ScriptingSystem to Player. Protect Kedama from being nuked during aggressive cleanup."!!KedamaExamplerPlayer class methodsFor: 'compiling' stamp: 'ar 2/27/2010 00:18'!isUniClass	"Uni-classes end with digits"	^self name endsWithDigit! !!KedamaTurtlePlayer class methodsFor: 'compiling' stamp: 'ar 2/27/2010 00:18'!isUniClass	"Uni-classes end with digits"	^self name endsWithDigit! !!Player class methodsFor: 'housekeeping' stamp: 'ar 2/27/2010 00:20'!cleanUp: aggressive	"Nuke uni-classes when aggressively cleaning up"	aggressive ifTrue:[		self withAllSubclassesDo:[:aClass|			aClass isUniClass ifTrue:[Smalltalk removeClassNamed: aClass name].		].	].! !