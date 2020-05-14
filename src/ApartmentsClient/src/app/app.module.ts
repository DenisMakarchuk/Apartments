import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

import {  ApartmentUserService,
          CommentUserService,
          OrderUserService,
          UserService,
          ApartmentSearchService,
          CommentAdministrationService,
          UserAdministrationService,
          MakeDatesArrayHelperService
        } from '../app/services/nswag.generated.service';
import { TokenInterceptorService } from './auth/token-interceptor.service';
import { LoggedService } from '../app/services/logged.service';
import { SearchParametersService } from '../app/services/search-parameters.service';

import { AppComponent } from './app.component';
import { MainMenuComponent } from './main-menu/main-menu.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';
import { SearchComponent } from './search/search.component';
import { SearchViewComponent } from './search/search-view/search-view.component';
import { SearchApartmentDetailComponent } from './search/search-apartment-detail/search-apartment-detail.component';
import { ProfilePanelComponent } from './profile/profile-panel/profile-panel.component';
import { OwnOrdersComponent } from './user/order/own-orders/own-orders.component';
import { OwnCommentsComponent } from './user/comment/own-comments/own-comments.component';
import { OwnCommentDetailComponent } from './user/comment/own-comment-detail/own-comment-detail.component';
import { OwnOrderDetailComponent } from './user/order/own-order-detail/own-order-detail.component';
import { OwnApartmentsComponent } from './user/apartment/own-apartments/own-apartments.component';
import { OwnApartmentDetailComponent } from './user/apartment/own-apartment-detail/own-apartment-detail.component';
import { AddApartmentComponent } from './user/apartment/add-apartment/add-apartment.component';
import { AddCommentComponent } from './user/comment/add-comment/add-comment.component';
import { ApartmentCommentsComponent } from './user/comment/apartment-comments/apartment-comments.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { UserAdministrationComponent } from './admin/user-administration/user-administration.component';
import { AdminAdministrationComponent } from './admin/admin-administration/admin-administration.component';
import { ProfileAdministrationDetailComponent } from './admin/profile-administration-detail/profile-administration-detail.component';
import { CommentsAdministrationComponent } from './admin/comments-administration/comments-administration.component';
import { AdministrationCommentDetailComponent } from './admin/administration-comment-detail/administration-comment-detail.component';
import { ApartmentOrdersComponent } from './user/order/apartment-orders/apartment-orders.component';


@NgModule({
  declarations: [
    AppComponent,
    MainMenuComponent,
    LogoutComponent,
    LoginComponent, 
    RegisterComponent, 
    SearchComponent, 
    SearchViewComponent, 
    SearchApartmentDetailComponent, 
    ProfilePanelComponent, 
    OwnOrdersComponent, 
    OwnCommentsComponent, 
    OwnCommentDetailComponent, 
    OwnOrderDetailComponent, OwnApartmentsComponent, OwnApartmentDetailComponent, AddApartmentComponent, AddCommentComponent, ApartmentCommentsComponent, AdminPanelComponent, UserAdministrationComponent, AdminAdministrationComponent, ProfileAdministrationDetailComponent, CommentsAdministrationComponent, AdministrationCommentDetailComponent, ApartmentOrdersComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true
    },
    ApartmentUserService,
    CommentUserService,
    OrderUserService,
    UserService,
    ApartmentSearchService,
    CommentAdministrationService,
    UserAdministrationService,
    LoggedService,
    SearchParametersService,
    MakeDatesArrayHelperService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
