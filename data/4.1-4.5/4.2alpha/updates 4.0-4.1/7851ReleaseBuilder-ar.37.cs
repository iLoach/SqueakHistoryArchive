"Change Set:		7851ReleaseBuilder-ar.37ReleaseBuilder-ar.37:Remove obsolete WorldWindow reference."!!ReleaseBuilderFor3dot11 methodsFor: 'cleaning' stamp: 'ar 9/19/2009 20:19'!cleanupPhasePrepareself cleanUnwantedCs.                "SMSqueakMap default clearCaches."" Commented out for no Undeclared on image "#(zapMVCprojects zapAllOtherProjects discardFlash discardFFIcomputeImageSegmentation discardSpeech ) do:[:ea| SystemDictionary removeSelector:ea].#( reserveUrl: saveAsResource saveDocPane saveOnURL saveOnURL:saveOnURLbasic isTurtleRow objectViewed inATwoWayScrollPane) do:[:ea| Morph removeSelector: ea].#(playfieldOptionsMenu presentPlayfieldMenu allScriptEditorsattemptCleanupReporting: modernizeBJProjectscriptorForTextualScript:ofPlayer:) do:[:ea| PasteUpMorph removeSelector:   ea].#(isUniversalTiles noteDeletionOf:fromWorld: scriptorsForSelector:inWorld: tilesToCall: handMeTilesToFire) do:[:ea| Player removeSelector:   ea].Player class removeCategory: 'turtles'.Player removeCategory: 'slots-user'.Morph removeCategory: 'scripting'.ColorType removeCategory: 'tiles'.TheWorldMainDockingBar removeSelector: #hideAllViewersIn: .SystemOrganization removeCategoriesMatching: 'UserObjects'.FileList2 class organization classify: #morphicViewOnDirectory: under: 'morphic ui'.FileList2 class organization classify: #morphicView under: 'morphic ui'.SystemOrganization classifyAll: #(AbstractMediaEventMorph ColorSwatch) under: 'MorphicExtras-AdditionalSupport'.! !