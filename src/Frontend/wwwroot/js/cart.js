




function refreshCartCounter() {    
    fetch('/Cart/CartNavPartial')
        .then(res => res.text())
        .then(html => {
            const container = document.getElementById('cart-summary');
            
            if (container) {
                container.innerHTML = '';
                container.insertAdjacentHTML('afterbegin', html);
            }
        })
        .catch(err => console.error("Failed to update cart", err));
}

async function addToCart(productId, apiUrl) {
    const url = `${apiUrl}/cart/increase/${productId}`;

    return $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ productId: productId, quantity: 1 }),
    })
        .done(function () {
            refreshCartCounter();
        })
        .fail(function () {
            console.error('Failed to increase quantity');
        });
}

async function removeFromCart(productId, apiUrl) {
    const url = `${apiUrl}/cart/decrease/${productId}`;

    return $.ajax({
        url: url,
        type: 'DELETE',
        contentType: 'application/json',
        data: JSON.stringify({ productId: productId, quantity: 1 }),
    })
        .done(function () {
            refreshCartCounter();
        })
        .fail(function () {
            console.error('Failed to decrease quantity');
        });
}
