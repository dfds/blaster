import jq from "jquery";
import Mustache from "mustache";

const emptyCallback = () => {};

export default class ModelEditor {

    constructor(element, options) {
        this.element = element;
        this.options = options;

        this.getFormData = this.getFormData.bind(this);
        this.handleCloseClick = this.handleCloseClick.bind(this);
        this.handleSaveClick = this.handleSaveClick.bind(this);
        this.assignEventHandlers = this.assignEventHandlers.bind(this);
        this.close = this.close.bind(this);
        this.focus = this.focus.bind(this);

        this.assignEventHandlers();
    }

    focus() {
        jq("[data-property]", this.element).focus();
    }

    assignEventHandlers() {
        const self = this;

        jq("[data-behavior=close]", this.element).click(function() { self.handleCloseClick(this); });
        jq("[data-behavior=save]", this.element).click(function() { self.handleSaveClick(this); });
    }

    getFormData() {
        const data = {};

        jq("[data-property]", this.element).each(function() {
            const key = jq(this).attr("data-property")
            const value = jq(this).val();

            data[key] = value;
        });

        return data;
    }

    handleCloseClick() {
        const callback = this.options.onClose || emptyCallback;
        callback();
    }

    handleSaveClick(button) {
        jq(button).addClass("is-loading");

        const data = this.getFormData();
        const callback = this.options.onSave || emptyCallback;
        callback(data);

        jq(button).removeClass("is-loading");
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

        return element;
    }

    static open(options) {
        const element = this.createElement(options);
        const editor = new ModelEditor(element, options);
        editor.focus();

        return editor;
    }
}