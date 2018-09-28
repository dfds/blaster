import jq from "jquery";
import Mustache from "mustache";

// export default class TeamEditor {
//     createEditorMarkupFor(dataItem) {
//         return new Promise(resolve => {
//             const editorTemplate = jq("#editor-template").html();
//             const markup = Mustache.render(editorTemplate, dataItem);
//             resolve(markup);
//         });
//     }

//     createEditorElementFrom(editorMarkup) {
//         return new Promise(resolve => {
//             const element = jq(editorMarkup);
//             jq(document.body).append(element);
//             resolve(element);
//         });
//     }

//     setFocusOnFirstElement(element) {
//         return new Promise(resolve => {
//             jq("[data-property=name]", element).focus();
//             resolve(element);
//         });
//     }

//     addButtonBehaviorTo(element) {
//         return new Promise((resolve, reject) => {
//             jq("[data-behavior=close]", element).click(() => {
//                 reject();
//                 element.remove();
//             });
//             jq("[data-behavior=save]", element).click(() => {
//                 resolve({
//                     name: jq("[data-property=name]", element).val(),
//                     department: jq("[data-property=department]", element).val()
//                 });
//                 element.remove();
//             });
//         });
//     }

//     open(item) {
//         return this.createEditorMarkupFor(item)
//             .then(this.createEditorElementFrom)
//             .then(this.setFocusOnFirstElement)
//             .then(this.addButtonBehaviorTo);
//     }
// }

export default class TeamEditor {

    constructor(element) {
        this.element = element;
    }

    close() {
        this.element.remove();
    }

    static createElement(options) {
        const templateContent = jq(options.template).html();
        const markup = Mustache.render(templateContent, options.data);
        const element = jq(markup);

        const container = options.container || document.body;
        jq(container).append(element);
        jq("[data-property=name]", element).focus();

        return element;
    }

    static open(options) {
        const element = this.createElement(options);
        const editor = new TeamEditor(element);

        jq("[data-behavior=close]", element).click(() => {
            if (options.onClose) {
                options.onClose();
            }
        });

        jq("[data-behavior=save]", element).click(() => {
            if (options.onSave) {
                options.onSave({
                    name: jq("[data-property=name]", element).val(),
                    department: jq("[data-property=department]", element).val()
                });
            }
        });

        return editor;
    }
}