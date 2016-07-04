"Change Set:		7771ToolBuilder-Kernel-ar.21ToolBuilder-Kernel-ar.21:Analyze menu labels to strip of the Morphic <on>/<off>/<yes>/<no> markers."!!PluggableMenuSpec methodsFor: 'construction' stamp: 'ar 9/7/2009 14:17'!analyzeItemLabels	"Analyze the item labels"	items do:[:item| item analyzeLabel].! !!PluggableMenuSpec methodsFor: 'construction' stamp: 'ar 9/7/2009 14:18'!buildWith: builder	self analyzeItemLabels.	^ builder buildPluggableMenu: self! !!PluggableMenuItemSpec methodsFor: 'initialize' stamp: 'ar 9/7/2009 14:24'!analyzeLabel	"For Morphic compatiblity. Some labels include markup such as <on>, <off> etc.	Analyze the label for these annotations and take appropriate action."	| marker |	marker := label copyFrom: 1 to: (label indexOf: $>).	(marker = '<on>' or:[marker = '<yes>']) ifTrue:[		checked := true.		label := label copyFrom: marker size+1 to: label size.	].	(marker = '<off>' or:[marker = '<no>']) ifTrue:[		checked := false.		label := label copyFrom: marker size+1 to: label size.	].! !