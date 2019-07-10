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
        this.showError = this.showError.bind(this);

        this.assignEventHandlers();
    }

    focus() {
        jq("[data-property]", this.element).first().focus();
    }

    nameValidation(evt) {
        const input = jq("[data-property=name]");
        const capNameValid = jq("#capNameValid");
        const nameValidationRegex = new RegExp('^[A-Z][a-zA-Z0-9\\-]{2,20}$');

        if (input.val().match(nameValidationRegex) == null) {
            capNameValid.css("color", "#f5426c");
        } else {
            capNameValid.css("color", "#33f58e");
        }
    }

    assignEventHandlers() {
        const self = this;

        jq("[data-behavior=close]", this.element).click(function() { self.handleCloseClick(this); });
        jq("[data-behavior=save]", this.element).click(function() { self.handleSaveClick(this); });
        jq("[data-property=name]", this.element).keyup(() => { self.nameValidation(this); });
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
        Promise.resolve()
            .then(() => jq(button).addClass("is-loading"))
            .then(() => this.getFormData())
            .then(data => {
                const callback = this.options.onSave || emptyCallback;
                return callback(data);        
            })
            .then(() => jq(button).removeClass("is-loading"));
    }

    showError(data) {
        const container = jq(".error", this.element);
        jq(".error-title", container).text(data.title);
        jq(".error-message", container).text(data.message);
        container.removeClass("is-hidden");

        setTimeout(function() {
            container.addClass("is-hidden");
        }, 2000);
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