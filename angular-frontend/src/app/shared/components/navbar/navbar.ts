import { Component, inject } from "@angular/core";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from "../../../core/services/auth";
import { CartService } from "../../../core/services/cart";


@Component({
    selector: 'app-navbar',
    standalone: true,
    imports: [RouterLink, RouterLinkActive],
    templateUrl: './navbar.html',
    styleUrl: './navbar.css'
})
export class NavbarComponent {
    auth = inject(AuthService);
    cart = inject(CartService);
}