"Change Set:		7723Graphics-ar.73Graphics-ar.73:Fixes a typo in m17n string display which would measure the width of the entire string instead of just the character.Graphics-ar.72:http://bugs.squeak.org/view.php?id=6936#asHTMLColor is inaccurate for certain values.Attached fix resolves that, also gives 10x speed-up. "!!StrikeFont methodsFor: 'displaying' stamp: 'ar 9/3/2009 08:15'!displayMultiString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta baselineY: baselineY	| leftX rightX glyphInfo char destY form gfont destX destPt |	destX := aPoint x.	charIndex := startIndex.	glyphInfo := Array new: 5.	startIndex to: stopIndex do:[:charIndex|		char := aString at: charIndex.		(self hasGlyphOf: char) ifTrue: [			self glyphInfoOf: char into: glyphInfo.			form := glyphInfo at: 1.			leftX := glyphInfo at: 2.			rightX := glyphInfo at: 3.			destY := glyphInfo at: 4.			gfont := glyphInfo at: 5.			(gfont == aBitBlt lastFont) ifFalse: [gfont installOn: aBitBlt].			destY := baselineY - destY. 			aBitBlt displayGlyph: form at: destX @ destY left: leftX right: rightX font: self.			destX := destX + (rightX - leftX + kernDelta).		] ifFalse:[			destPt := self fallbackFont displayString: aString on: aBitBlt from: charIndex to: charIndex at: destX @ aPoint y kern: kernDelta from: self baselineY: baselineY.			destPt x = destX ifTrue:[				"In some situations BitBlt doesn't return the advance width from the primitive.				Work around the situation"				destX := destX + (self widthOfString: aString from: charIndex to: charIndex) + kernDelta.			] ifFalse:[destX := destPt x].		].	].	^destX @ aPoint y! !!Color methodsFor: 'conversions' stamp: 'bf 2/19/2008 12:10'!asHTMLColor	| s |	s := '#000000' copy.	s at: 2 put: (Character digitValue: ((rgb bitShift: -6 - RedShift) bitAnd: 15)).	s at: 3 put: (Character digitValue: ((rgb bitShift: -2 - RedShift) bitAnd: 15)).	s at: 4 put: (Character digitValue: ((rgb bitShift: -6 - GreenShift) bitAnd: 15)).	s at: 5 put: (Character digitValue: ((rgb bitShift: -2 - GreenShift) bitAnd: 15)).	s at: 6 put: (Character digitValue: ((rgb bitShift: -6 - BlueShift) bitAnd: 15)).	s at: 7 put: (Character digitValue: ((rgb bitShift: -2 - BlueShift) bitAnd: 15)).	^ s! !Color class removeSelector: #hex:!