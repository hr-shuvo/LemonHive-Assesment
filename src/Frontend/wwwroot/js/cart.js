


// document.addEventListener("DOMContentLoaded", function() {
//     loadCart().then(r => r);
// })

async function loadCart() {
    const url = `${window.apiUrl}/cart`;
    
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (cart) {
                updateCartSpans(cart);
                resolve(cart);
            },
            error: function (err) {
                console.error('Error loading cart', err);
                reject(err);
            }
        });
    })
    
    
    
}

function updateCartSpans(cart) {
    $('.cart-qty').each(function () {
        const productId = $(this).data('product-id');
        const item = cart.items.find(i => i.productId === productId);

        const itemCountDiv = $(`#product-item-count-${productId}`);
        const addCartDiv = $(`#product-add-cart-${productId}`);


        if (item) {
            $(this).text(item.quantity);
            itemCountDiv.show();
            addCartDiv.hide();
        } else {
            itemCountDiv.hide();
            addCartDiv.show();
        }
    });
}








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
            loadCart();
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
            loadCart();
        })
        .fail(function () {
            console.error('Failed to decrease quantity');
        });
}
