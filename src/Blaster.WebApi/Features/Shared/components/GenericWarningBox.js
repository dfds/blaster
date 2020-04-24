import Vue from "vue";

const GenericWarningBoxComponent = Vue.component("generic-warning-box", {
	props: ["err"],
	mounted: function () {
	},
	data: function () {
		return {

		}
	},
	watch: {

	},
	computed: {
    msg: function() {
      if (this.err) {
        if (this.statusCode >= 500 && this.statusCode <= 599) {
          return `Internal Server Error! If there isn't already an open incident on <a href="https://dfdsit.statuspage.io/">dfdsit.statuspage.io</a> and the error still persists after a five minute waiting period, please contact us on #dev-excellence or in-person.`;
        }
        return this.err.response.data.message;
      } else {
        return "";
      }
    },
    statusCode: function() {
      if (this.err) {
        return this.err.response.status;
      } else {
        return -1;
      }
    },
    activeError: function() {
      return this.err ? true : false;
    },
    color: function() {
      if (this.err) {
        if (this.statusCode >= 400 && this.statusCode <= 499) {
          return "orange";
        } else {
          return "red";
        }
      } else {
        return "";
      }
    }
	},
	methods: {

	},
  template: `
    <div v-if="activeError" class="generic-warning-box" v-bind:class="[color]">
      <p style="word-wrap: anywhere" v-html="msg"></p>
    </div>
    `
})

export default GenericWarningBoxComponent;
export {GenericWarningBoxComponent};