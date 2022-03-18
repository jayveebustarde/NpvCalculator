(function () {
    'use strict';

    angular
        .module('app')
        .factory('ApiService', ApiService);

    function ApiService($http) {
        let service = {
            calculateNpv: callCalculateNpv
        };

        return service;

        /////////////////////////////////////////
        function callCalculateNpv(data) {
            return $http({
                method: 'POST',
                url: '/api/calculation',
                data: data
            });
        }
    }
})();