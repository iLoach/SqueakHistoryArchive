"Change Set:		7498Collections-ar.114Collections-ar.114:ToolBuilder cleanup for Transcript opening."!!TranscriptStream class methodsFor: 'as yet unclassified' stamp: 'ar 8/7/2009 22:24'!openMorphicTranscript	"Have the current project's transcript open up as a morph"	^ToolBuilder open: self! !!TranscriptStream methodsFor: 'initialization' stamp: 'ar 8/7/2009 22:28'!openLabel: aString 	"Open a window on this transcriptStream"	^ToolBuilder open: self label: aString! !TranscriptStream removeSelector: #openAsMorphLabel:!TranscriptStream removeSelector: #openAsMorph!