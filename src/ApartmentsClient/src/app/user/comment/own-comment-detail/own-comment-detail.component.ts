import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { CommentUserService, CommentDTO } from 'src/app/services/nswag.generated.service'
import { ActivatedRoute } from '@angular/router';
import { LoggedService } from 'src/app/services/logged.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-own-comment-detail',
  templateUrl: './own-comment-detail.component.html',
  styleUrls: ['./own-comment-detail.component.scss']
})
export class OwnCommentDetailComponent implements OnInit {

  spinning = false;
  spinningDel = false;
  errorMessage: string;

  comment: CommentDTO;
  isUpdating = false;

  constructor(
    private authService: LoggedService,
    private location: Location,
    private route: ActivatedRoute,
    private commentService: CommentUserService
  ) { }

  ngOnInit(): void {
    this.getComment();
  }

  get isAdmin(){
    var token = this.authService.getToken();
        
    var decodedoken = jwt_decode(token);
    var currentRole = decodedoken['role'];

    if (currentRole.includes('Admin')) {
      return true;
    }
    return false;
  }

  get isAuthor(){
    var token = this.authService.getToken();
        
    var decodedoken = jwt_decode(token);
    var currentUserId = decodedoken['id'];

    if (this.comment.authorId = currentUserId) {
      return true;
    }
    return false;
  }

  getComment(){
    this.errorMessage = null;
    this.spinning = true;

    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.getCommentById(id)
    .subscribe(comment => {

      this.spinning = false;
      this.comment = comment;
    },
    error=>{
      this.spinning = false;
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      if (error.status ===  404) {
        this.errorMessage = "Error 404: " + error.response;
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    })
  }

  updating(){
    this.isUpdating = true;
  }

  save(): void {
    this.errorMessage = null;
    this.spinning = true;

    this.commentService.updateComment(this.comment)
      .subscribe(comment => {

        this.spinning = false;
        this.comment = comment;
        this.isUpdating = false;
    },
    error=>{
      this.spinning = false;
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  delete(){
    this.errorMessage = null;
    this.spinningDel = true;

    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.deleteCommentById(id)
    .subscribe(()=>{

      this.spinningDel = false;
      this.goBack()
    },
    error=>{
      this.spinningDel = false;
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      if (error.status ===  404) {
        this.errorMessage = "Error 404: " + error.response;
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  goBack(){
    if (this.isUpdating) {
      this.isUpdating = false;
    }
    else{
      this.location.back();
    }
  }

}
