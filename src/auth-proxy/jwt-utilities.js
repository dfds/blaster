class JWTUtilities {
    getEmail(jwt) {
        if (jwt == undefined) {
            return "";
        }
    
        var jwtParts = jwt.split(".");
    
        if (jwtParts.length != 3) {
            return "";
        }
    
        var userPartBase64 = jwtParts[1];
        var userPart = JSON.parse(this.base64Decode(userPartBase64));
        
        return userPart.email;
    }
    
    base64Decode(str, encoding = 'utf-8') {
        let buff = Buffer.from(str, 'base64');  
        let text = buff.toString(encoding);
        
        return text;
    }
}

module.exports = JWTUtilities;
