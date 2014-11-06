mySettings = {
	previewAutoRefresh: false,
	previewParser: function (content) {
		return $.fn.bbcodeParser(content);
	},
	markupSet: [
		{ name: 'Жирный', key: 'B', openWith: '[b]', closeWith: '[/b]' },
		{ name: 'Наклонный', key: 'I', openWith: '[i]', closeWith: '[/i]' },
		{ name: 'Подчеркнутый', key: 'U', openWith: '[u]', closeWith: '[/u]' },
		{ name: 'Зачеркнутый', key: 'S', openWith: '[s]', closeWith: '[/s]' },
		{ name: 'Верхний индекс', key: '.', openWith: '[sup]', closeWith: '[/sup]' },
		{ name: 'Нижний индекс', key: ',', openWith: '[sub]', closeWith: '[/sub]' },
		{ separator: '---------------' },
		{ name: 'Выровнять по ширине', openWith: '[align=justify]', closeWith: '[/align]' },
		{ name: 'Выровнять вправо', openWith: '[align=right]', closeWith: '[/align]' },
		{ name: 'Выровнять по центру', openWith: '[align=center]', closeWith: '[/align]' },
		{ name: 'Выровнять влево', openWith: '[align=left]', closeWith: '[/align]' },
		{ separator: '---------------' },
		{ name: 'Картинка', key: 'P', replaceWith: '[img][![Адрес картинки::!:http://]!][/img]' },
		{ name: 'Ссылка', key: 'L', openWith: '[url=[![Адрес сайта::!:http://]!]]', closeWith: '[/url]' },
		{ name: 'Youtube', key: 'Y', replaceWith: '[youtube][![Youtube адрес::!:http://]!][/youtube]' },
		{ separator: '---------------' },
		{
			name: 'Шрифт',
			dropMenu: [
				{ name: 'Courier', openWith: '[font="Courier"]', closeWith: '[/font]', className: 'font-family-1' },
				{ name: 'Courier New', openWith: '[font="Courier New"]', closeWith: '[/font]', className: 'font-family-2' },
				{ name: 'Arial', openWith: '[font="Arial"]', closeWith: '[/font]', className: 'font-family-3' },
				{ name: 'Fixedsys', openWith: '[font="Fixedsys"]', closeWith: '[/font]', className: 'font-family-4' },
				{ name: 'Comic Sans MS', openWith: '[font="Comic Sans MS"]', closeWith: '[/font]', className: 'font-family-5' },
				{ name: 'Georgia', openWith: '[font="Georgia"]', closeWith: '[/font]', className: 'font-family-6' },
				{ name: 'Tahoma', openWith: '[font="Tahoma"]', closeWith: '[/font]', className: 'font-family-7' },
				{ name: 'Times New Roman', openWith: '[font="Times New Roman"]', closeWith: '[/font]', className: 'font-family-8' }
			]
		},
		{
			name: 'Размер шрифта', openWith: '[size=[![Размер шрифта(px):]!]]', closeWith: '[/size]',
			dropMenu: [
				{ name: 'Маленький', openWith: '[size=9]', closeWith: '[/size]', className: 'font-small' },
				{ name: 'Обычный', openWith: '[size=13]', closeWith: '[/size]', className: 'font-normal' },
				{ name: 'Большой', openWith: '[size=17]', closeWith: '[/size]', className: 'font-big' }
			]
		},
		{
			name: 'Цвет шрифта', className: 'palette', openWith: '[color=[![Код цвета(hex)::!:#]!]]', closeWith: '[/color]',
			dropMenu: [
				{ name: 'Light Yellow', openWith: '[color=#FCE94F]', closeWith: '[/color]', className: "col1-1" },
				{ name: 'Yellow', openWith: '[color=#EDD400]', closeWith: '[/color]', className: "col1-2" },
				{ name: 'Dark Yellow', openWith: '[color=#C4A000]', closeWith: '[/color]', className: "col1-3" },

				{ name: 'Light Orange', openWith: '[color=#FCAF3E]', closeWith: '[/color]', className: "col2-1" },
				{ name: 'Orange', openWith: '[color=#F57900]', closeWith: '[/color]', className: "col2-2" },
				{ name: 'Dark Orange', openWith: '[color=#CE5C00]', closeWith: '[/color]', className: "col2-3" },

				{ name: 'Light Brown', openWith: '[color=#E9B96E]', closeWith: '[/color]', className: "col3-1" },
				{ name: 'Brown', openWith: '[color=#C17D11]', closeWith: '[/color]', className: "col3-2" },
				{ name: 'Dark Brown', openWith: '[color=#8F5902]', closeWith: '[/color]', className: "col3-3" },

				{ name: 'Light Green', openWith: '[color=#8AE234]', closeWith: '[/color]', className: "col4-1" },
				{ name: 'Green', openWith: '[color=#73D216]', closeWith: '[/color]', className: "col4-2" },
				{ name: 'Dark Green', openWith: '[color=#4E9A06]', closeWith: '[/color]', className: "col4-3" },

				{ name: 'Light Blue', openWith: '[color=#729FCF]', closeWith: '[/color]', className: "col5-1" },
				{ name: 'Blue', openWith: '[color=#3465A4]', closeWith: '[/color]', className: "col5-2" },
				{ name: 'Dark Blue', openWith: '[color=#204A87]', closeWith: '[/color]', className: "col5-3" },

				{ name: 'Light Purple', openWith: '[color=#AD7FA8]', closeWith: '[/color]', className: "col6-1" },
				{ name: 'Purple', openWith: '[color=#75507B]', closeWith: '[/color]', className: "col6-2" },
				{ name: 'Dark Purple', openWith: '[color=#5C3566]', closeWith: '[/color]', className: "col6-3" },

				{ name: 'Light Red', openWith: '[color=#EF2929]', closeWith: '[/color]', className: "col7-1" },
				{ name: 'Red', openWith: '[color=#CC0000]', closeWith: '[/color]', className: "col7-2" },
				{ name: 'Dark Red', openWith: '[color=#A40000]', closeWith: '[/color]', className: "col7-3" },

				{ name: 'White', openWith: '[color=#FFFFFF]', closeWith: '[/color]', className: "col8-1" },
				{ name: 'Gray', openWith: '[color=#BABDB6]', closeWith: '[/color]', className: "col8-3" },
				{ name: 'Black', openWith: '[color=#000000]', closeWith: '[/color]', className: "col9-3" }
			]
		},
		{
			name: 'Смайлики', className: 'emoticon', dropMenu: [
				{ name: 'В разработке', openWith: '', className: '' }
			]
		},
		{ separator: '---------------' },
		{ name: 'Список', openWith: '[list]\n', closeWith: '\n[/list]' },
		{ name: 'Элемент списка', openWith: '[*] ' },
		{ separator: '---------------' },
		{ name: 'Цитата', openWith: '[quote]', closeWith: '[/quote]' },
		{ name: 'Код', openWith: '[code]', closeWith: '[/code]' },
		{ name: 'Без изменений', openWith: '[noparse]', closeWith: '[/noparse]' },
		{ name: 'Спойлер', openWith: '[spoiler]', closeWith: '[/spoiler]' },
		{ name: 'Off top', openWith: '[off]', closeWith: '[/off]' },
		{ name: 'Горизонтальная линия', openWith: '[hr]' }
	]
}