import jq from "jquery";
import Mustache from "Mustache";

export default class AlertDialog {
    createMarkupFor(dataItem) {
        return new Promise(resolve => {
            const editorTemplate = jq("#alert-template").html();
            const markup = Mustache.render(editorTemplate, dataItem);
            resolve(markup);
        });
    }

    createDOMElementFrom(markup) {
        return new Promise(resolve => {
            // remove alert if currently active
            const alertDialog = document.getElementById("alert-dialog");
            if (alertDialog) {
                alertDialog.remove();
            }

            const element = jq(markup);
            const alertContainer = document.getElementById("alert-container") || document.body;
            
            jq(alertContainer).append(element);
            resolve(element);
        });
    }

    addCloseBehaviorTo(element) {
        const module = this;

        return new Promise((resolve, reject) => {
            jq("[data-behavior=close]", element).click(() => {
                reject();
                element.remove();
            });

            resolve(element);
        });
    }

    close(element) {
        element.remove();
    }

    open(data) {
        return this.createMarkupFor(data)
            .then(this.createDOMElementFrom)
            .then(this.setFocusOnFirstElement)
            .then(this.addCloseBehaviorTo);
    }
}