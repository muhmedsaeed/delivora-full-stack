import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';


@Injectable({ providedIn: 'root' })
export class ApiService {
    protected http = inject(HttpClient);
    protected baseUrl = environment.apiUrl;
}