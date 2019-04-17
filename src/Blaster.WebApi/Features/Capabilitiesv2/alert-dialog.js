import jq from "jquery";
import Mustache from "mustache";

export default class AlertDialog {

    constructor(element) {

        this.element = element;
    }

    close() {
        this.element.remove();
    }

    static createElement(options) {
        const editorTemplate = jq(options.template).html();
        const markup = Mustache.render(editorTemplate, options.data);
        const element = jq(markup);

        const alertContainer = options.container || document.body;
        jq(alertContainer).append(element);

        return element;
    }

    static open(options) {
        const element = this.createElement(options);
        const dialog = new AlertDialog(element);

        // add behavior
        jq("[data-behavior=close]", element).click(() => {
            dialog.close();
        });

        return dialog;
    }
}