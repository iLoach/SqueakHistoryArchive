"Change Set:		7924ST80-ar.56ST80-ar.56:Fix missing return in StringHolder utility method."!!StringHolder class methodsFor: 'yellow button menu' stamp: 'ar 10/6/2009 10:36'!codePaneMenu: aMenu shifted: shifted	"Utility method for the 'standard' codePane menu"	aMenu addList: (shifted 		ifTrue:[self shiftedYellowButtonMenuItems]		ifFalse:[self yellowButtonMenuItems]).	^aMenu! !