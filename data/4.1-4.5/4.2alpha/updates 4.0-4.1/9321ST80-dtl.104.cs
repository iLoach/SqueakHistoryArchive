"Change Set:		9321ST80-dtl.104ST80-dtl.104:Move PopUpMenu>>computeLabelParagraph from package Tools to ST80.Move StandardFileMenu>>computeLabelParagraph from package Tools to ST80."!!PopUpMenu methodsFor: '*ST80-Support' stamp: ''!computeLabelParagraph	"Answer a Paragraph containing this menu's labels, one per line and centered."	^ Paragraph withText: labelString asText style: MenuStyle! !!StandardFileMenu methodsFor: '*ST80-Support' stamp: 'acg 4/15/1999 20:50'!computeLabelParagraph	"Answer a Paragraph containing this menu's labels, one per line and centered."	^ Paragraph withText: labelString asText style: (MenuStyle leftFlush)! !