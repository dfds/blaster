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
        
        return {
            id: userPart.email,
            name: userPart.name,
            email: userPart.email
        };
    }
    
    base64Decode(str, encoding = 'utf-8') {
        let buff = Buffer.from(str, 'base64');  
        let text = buff.toString(encoding);
        
        return text;
    }
}

module.exports = JWTUtilities;
