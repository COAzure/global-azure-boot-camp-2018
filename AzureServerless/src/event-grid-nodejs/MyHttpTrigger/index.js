module.exports = function (context, req) {

    context.log("**** Body ****")
    context.log(req.body[0])

    context.log("**** Headers ****")
    context.log(req.headers)

    message = "";
    if (req.body[0].data != undefined) {
        if (req.body[0].data.validationCode) {
            context.res = {
                body: {
                    "validationResponse": req.body[0].data.validationCode
                }
            };
        }

        if (req.body[0].eventType) {
            if (req.body[0].eventType == "CustomerCreated") {
                message = req.body[0].data.TenantId + " signed up!";
            }

            if (req.body[0].eventType == "CustomerPaid") {
                message = req.body[0].data.TenantId + " finally paid!";
            }

            if (req.body[0].eventType == "CustomerStarted") {
                message = req.body[0].data.FirstName + " " + req.body[0].data.LastName + " is using the service!";
            }
        }
    }

    context.bindings.message = {};
    context.bindings.message = {
        body: 'Whoo! ' + message,
        to: 'TODO'
    };

    context.done();
};