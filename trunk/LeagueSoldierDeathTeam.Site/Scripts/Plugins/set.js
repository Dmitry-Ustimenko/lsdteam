mySettings = {
	previewParser: function (content) {
		return XBBCODE.process({
			text: content,
			addInLineBreaks: true
		}).html;
	},
	previewParserPath: "templates/preview.html",
	markupSet: [
		{ name: 'Bold', key: 'B', openWith: '[b]', closeWith: '[/b]' },
		{ name: 'Italic', key: 'I', openWith: '[i]', closeWith: '[/i]' },
		{ name: 'Underline', key: 'U', openWith: '[u]', closeWith: '[/u]' },
		{ name: 'Strike', key: 'S', openWith: '[s]', closeWith: '[/s]' },
		{ name: 'Sup', key: '.', openWith: '[sup]', closeWith: '[/sup]' },
		{ name: 'Sub', key: ',', openWith: '[sub]', closeWith: '[/sub]' },
		{ separator: '---------------' },
		{ name: 'Justify', openWith: '[align=justify]', closeWith: '[/align]' },
		{ name: 'Right', openWith: '[align=right]', closeWith: '[/align]' },
		{ name: 'Center', openWith: '[align=center]', closeWith: '[/align]' },
		{ name: 'Left', openWith: '[align=left]', closeWith: '[/align]' },
		{ separator: '---------------' },
		{ name: 'Picture', key: 'P', replaceWith: '[img][![Image Url]!][/img]' },
		{ name: 'Link', key: 'L', openWith: '[url=[![Site Url]!]]', closeWith: '[/url]', placeHolder: '' },
		{ separator: '---------------' },
		{
			name: 'Size', openWith: '[size=[![Text size]!]]', closeWith: '[/size]',
			dropMenu: [
				{ name: 'Big', openWith: '[size=9]', closeWith: '[/size]' },
				{ name: 'Normal', openWith: '[size=13]', closeWith: '[/size]' },
				{ name: 'Small', openWith: '[size=17]', closeWith: '[/size]' }
			]
		},
		{
			name: 'Colors', className: 'palette', dropMenu: [
				{ name: 'Yellow', openWith: '[col=#FCE94F]', closeWith: '[/col]', className: "col1-1" },
				{ name: 'Yellow', openWith: '[col=#EDD400]', closeWith: '[/col]', className: "col1-2" },
				{ name: 'Yellow', openWith: '[col=#C4A000]', closeWith: '[/col]', className: "col1-3" },

				{ name: 'Orange', openWith: '[col=#FCAF3E]', closeWith: '[/col]', className: "col2-1" },
				{ name: 'Orange', openWith: '[col=#F57900]', closeWith: '[/col]', className: "col2-2" },
				{ name: 'Orange', openWith: '[col=#CE5C00]', closeWith: '[/col]', className: "col2-3" },

				{ name: 'Brown', openWith: '[col=#E9B96E]', closeWith: '[/col]', className: "col3-1" },
				{ name: 'Brown', openWith: '[col=#C17D11]', closeWith: '[/col]', className: "col3-2" },
				{ name: 'Brown', openWith: '[col=#8F5902]', closeWith: '[/col]', className: "col3-3" },

				{ name: 'Green', openWith: '[col=#8AE234]', closeWith: '[/col]', className: "col4-1" },
				{ name: 'Green', openWith: '[col=#73D216]', closeWith: '[/col]', className: "col4-2" },
				{ name: 'Green', openWith: '[col=#4E9A06]', closeWith: '[/col]', className: "col4-3" },

				{ name: 'Blue', openWith: '[col=#729FCF]', closeWith: '[/col]', className: "col5-1" },
				{ name: 'Blue', openWith: '[col=#3465A4]', closeWith: '[/col]', className: "col5-2" },
				{ name: 'Blue', openWith: '[col=#204A87]', closeWith: '[/col]', className: "col5-3" },

				{ name: 'Purple', openWith: '[col=#AD7FA8]', closeWith: '[/col]', className: "col6-1" },
				{ name: 'Purple', openWith: '[col=#75507B]', closeWith: '[/col]', className: "col6-2" },
				{ name: 'Purple', openWith: '[col=#5C3566]', closeWith: '[/col]', className: "col6-3" },

				{ name: 'Red', openWith: '[col=#EF2929]', closeWith: '[/col]', className: "col7-1" },
				{ name: 'Red', openWith: '[col=#CC0000]', closeWith: '[/col]', className: "col7-2" },
				{ name: 'Red', openWith: '[col=#A40000]', closeWith: '[/col]', className: "col7-3" },

				{ name: 'Gray', openWith: '[col=#FFFFFF]', closeWith: '[/col]', className: "col8-1" },
				{ name: 'Gray', openWith: '[col=#D3D7CF]', closeWith: '[/col]', className: "col8-2" },
				{ name: 'Gray', openWith: '[col=#BABDB6]', closeWith: '[/col]', className: "col8-3" },

				{ name: 'Gray', openWith: '[col=#888A85]', closeWith: '[/col]', className: "col9-1" },
				{ name: 'Gray', openWith: '[col=#555753]', closeWith: '[/col]', className: "col9-2" },
				{ name: 'Gray', openWith: '[col=#000000]', closeWith: '[/col]', className: "col9-3" }
			]
		},
		{ separator: '---------------' },
		{ name: 'Bulleted list', openWith: '[list]\n', closeWith: '\n[/list]' },
		{ name: 'Numeric list', openWith: '[list=[![Starting number]!]]\n', closeWith: '\n[/list]' },
		{ name: 'List item', openWith: '[*] ' },
		{ separator: '---------------' },
		{ name: 'Quotes', openWith: '[quote]', closeWith: '[/quote]' },
		{ name: 'Code', openWith: '[code]', closeWith: '[/code]' },
		{ name: 'Preview', className: 'preview', call: 'preview' },
	]
}