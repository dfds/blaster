import jq from "jquery";
import Mustache from "mustache";

export default class AlertDialog {
    constructor(element, options) {
        this.element = element;
        this.options = options;

        this.assignEventHandlers = this.assignEventHandlers.bind(this);
        this.setupAutoClose = this.setupAutoClose.bind(this);
        this.close = this.close.bind(this);

        this.assignEventHandlers();
        this.setupAutoClose();
    }

    assignEventHandlers() {
        const self = this;
        jq("[data-behavior=close]", this.element).click(() => self.close());
    }

    setupAutoClose() {
        const autoCloseTimeout = this.options.autoClose || 0;
        
        if (autoCloseTimeout > 0) {
            const self = this;
            setTimeout(() => self.close(), autoCloseTimeout);
        }
    }

    close() {
        this.element.remove();
    }

    static createElement(options) {
        const template = jq(options.template).html();
        const markup = Mustache.render(template, options.data);
        const element = jq(markup);

        const alertContainer = options.container || document.body;
        jq(alertContainer).append(element);

        return element;
    }

    static open(options) {
        const element = this.createElement(options);
        return new AlertDialog(element, options);
    }
}