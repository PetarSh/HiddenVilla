redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51JNcMgGJ9UUVac2WQ6msph82u1yblSNDhirfQ9fF0zMZD87iwgBUf6iZGV6juccWa0RBUiSvm1HflfxqZnnjLAyK00BHNMTcCE');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};