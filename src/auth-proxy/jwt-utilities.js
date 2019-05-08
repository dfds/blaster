class JWTUtilities {
    getUserInformation(jwt) {
        if (jwt == undefined) {
            return null;
        }

        let jwtParts = jwt.split(".");

        /// JWT should consist of 3 parts.
        if (jwtParts.length != 3) {
            return null;
        }

        let userPartBase64 = jwtParts[1];
        let userPart = JSON.parse(this.base64Decode(userPartBase64));

        let email = (userPart.email != undefined) ? userPart.email : userPart.upn;

        return {
            id: email,
            name: userPart.name,
            email: email
        };
    }

    base64Decode(str, encoding = 'utf-8') {
        let buffer = Buffer.from(str, 'base64');
        let text = buffer.toString(encoding);

        return text;
    }

    base64Encode(str, encoding = 'base64') {
        let buffer = Buffer.from(str);
        let text = buffer.toString(encoding);

        return text;
    }
}

module.exports = JWTUtilities;
