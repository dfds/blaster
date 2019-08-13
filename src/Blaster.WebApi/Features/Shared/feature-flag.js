export default class FeatureFlag {
    constructor() {
        this.flags = {};
        this.setupFlags();
    }

    setupFlags() {
         this.addFlag(new FlagTestFilter());
         this.addFlag(new FlagTopics());
    }

    flagExists(name) {
        return this.flags[name] !== null;
    }

    addFlag(flag) {
        this.flags[flag.name] = flag;
    }

    getFlag(name) {
        return this.flags[name];
    }
}

class Flag {
    constructor() {
        this.enabled = false;
        this.name = '';
    }

    init() {
        const param_name = 'ff_' + this.name;
        const queryParam = new URLSearchParams(window.location.search).get(param_name);
        this.enabled = queryParam !== null;
    }
}

class FlagTestFilter extends Flag {
    constructor() {
        super();
        this.name = 'testfilter';
        this.init();
    }
}

class FlagTopics extends Flag {
    constructor() {
        super();
        this.name = 'topics';
        this.init();
    }
}

export {FeatureFlag, Flag};