"Change Set:		8025TrueType-nice.11TrueType-nice.11:Use #keys rather than #fasterKeysNote that pattern (x keys asArray sort) could as well be written (x keys sort) now that keys returns an Array...This #asArray is here solely for cross-dialect/fork compatibility."!!TTFileDescription class methodsFor: 'user interaction' stamp: 'ar 7/30/2009 21:31'!fontFromUser: priorFont allowKeyboard: aBoolean	"TTFileDescription fontFromUser"	| fontMenu active ptMenu label fontNames builder resultBlock result item style font widget |	builder := ToolBuilder default.	fontNames := self allFontsAndFiles keys asArray sort.	fontMenu := builder pluggableMenuSpec new.	fontMenu label: 'Non-portable fonts'.	resultBlock := [:value| result := value].	fontNames do: [:fontName |		active := priorFont familyName sameAs: fontName.		ptMenu := builder pluggableMenuSpec new.		TTCFont pointSizes do: [:pt |			label := pt printString, ' pt'.			item := ptMenu add: label 				target: resultBlock				selector: #value:				argumentList: {{fontName. pt}}.			item checked: (active and:[pt = priorFont pointSize]).		].		item := fontMenu add: fontName action: nil.		item subMenu: ptMenu.		item checked: active.	].	widget := builder open: fontMenu.	builder runModal: widget.	result ifNil:[^nil].	style := (TextStyle named: result first) ifNil:[self installFamilyNamed: result first].	style ifNil: [^ self].	font := style fonts detect: [:any | any pointSize = result last] ifNone: [nil].	^ font! !