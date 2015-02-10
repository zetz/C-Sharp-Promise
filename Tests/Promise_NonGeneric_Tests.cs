﻿using Moq;
using RSG.Promise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Promise.Tests
{
    public class Promise_NonGeneric_Tests
    {
        [Fact]
        public void can_resolve_simple_promise()
        {
            var promise = Promise.Resolved();

            var completed = 0;
            promise.Done(() => ++completed);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_reject_simple_promise()
        {
            var ex = new Exception();
            var promise = Promise.Rejected(ex);

            var errors = 0;
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++errors;
            });

            Assert.Equal(1, errors);
        }

        [Fact]
        public void can_resolve_promise_and_trigger_completed_handler()
        {
            var promise = new Promise();

            var completed = 0;

            promise.Done(() => ++completed);

            promise.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void exception_is_thrown_for_resolve_after_resolve()
        {
            var promise = new Promise();

            promise.Resolve();

            Assert.Throws<ApplicationException>(() =>
                promise.Resolve()
            );
        }

        [Fact]
        public void can_resolve_promise_and_trigger_multiple_completed_handlers()
        {
            var promise = new Promise();

            var completed1 = 0;
            var completed2 = 0;

            promise.Done(() => ++completed1);
            promise.Done(() => ++completed2);

            promise.Resolve();

            Assert.Equal(1, completed1);
            Assert.Equal(1, completed2);
        }

        [Fact]
        public void can_resolve_promise_and_trigger_completed_handler_with_registration_after_resolve()
        {
            var promise = new Promise();

            var completed = 0;

            promise.Resolve();

            promise.Done(() => ++completed);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_resolve_promise_and_trigger_multiple_completed_handlers_with_registration_after_resolve()
        {
            var promise = new Promise();

            var completed1 = 0;
            var completed2 = 0;

            promise.Resolve();

            promise.Done(() => ++completed1);
            promise.Done(() => ++completed2);            

            Assert.Equal(1, completed1);
            Assert.Equal(1, completed2);
        }

        [Fact]
        public void can_resolve_with_value_and_trigger_completed_handler()
        {
            var promise = new Promise();

            var completed = 0;

            promise.Done(() => ++completed);

            promise.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_resolve_with_value_and_trigger_multiple_completed_handlers()
        {
            var promise = new Promise();

            var completed1 = 0;
            var completed2 = 0;

            promise.Done(() => ++completed1);
            promise.Done(() => ++completed2);

            promise.Resolve();

            Assert.Equal(1, completed1);
            Assert.Equal(1, completed2);
        }

        [Fact]
        public void can_resolve_with_value_and_trigger_completed_handler_with_registration_after_resolve()
        {
            var promise = new Promise();

            var completed = 0;

            promise.Done(() => ++completed);

            promise.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_resolve_with_value_and_trigger_multiple_completed_handlers_with_registration_after_resolve()
        {
            var promise = new Promise();

            var completed1 = 0;
            var completed2 = 0;

            promise.Resolve();

            promise.Done(() => ++completed1);
            promise.Done(() => ++completed2);

            Assert.Equal(1, completed1);
            Assert.Equal(1, completed2);
        }

        [Fact]
        public void can_reject_promise_and_trigger_error_handler()
        {
            var promise = new Promise();

            var ex = new ApplicationException();
            var completed = 0;
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed;
            });

            promise.Reject(ex);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void exception_is_thrown_for_reject_after_reject()
        {
            var promise = new Promise();

            promise.Reject(new ApplicationException());

            Assert.Throws<ApplicationException>(() =>
                promise.Reject(new ApplicationException())
            );
        }

        [Fact]
        public void exception_is_thrown_for_reject_after_resolve()
        {
            var promise = new Promise();

            promise.Resolve();

            Assert.Throws<ApplicationException>(() =>
                promise.Reject(new ApplicationException())
            );
        }

        [Fact]
        public void exception_is_thrown_for_resolve_after_reject()
        {
            var promise = new Promise();

            promise.Reject(new ApplicationException());

            Assert.Throws<ApplicationException>(() =>
                promise.Resolve()                
            );
        }

        [Fact]
        public void can_reject_promise_and_trigger_multiple_error_handlers()
        {
            var promise = new Promise();

            var ex = new ApplicationException();
            var completed1 = 0;
            var completed2 = 0;
            promise.Catch(e => 
            {
                Assert.Equal(ex, e);
                ++completed1;
            });
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed2;
            });

            promise.Reject(ex);

            Assert.Equal(1, completed1);
            Assert.Equal(1, completed2);
        }

        [Fact]
        public void can_reject_promise_and_trigger_error_handler_with_registration_after_reject()
        {
            var promise = new Promise();

            var ex = new ApplicationException();
            promise.Reject(ex);

            var completed = 0;
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed;
            });

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_reject_promise_and_trigger_multiple_error_handlers_with_registration_after_reject()
        {
            var promise = new Promise();

            var ex = new ApplicationException();
            promise.Reject(ex);

            var completed1 = 0;
            var completed2 = 0;
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed1;
            });
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed2;
            });

            Assert.Equal(1, completed1);
            Assert.Equal(1, completed2);
        }

        [Fact]
        public void error_handler_is_not_invoked_for_resolved_promised()
        {
            var promise = new Promise();

            promise.Catch(e =>
            {
                throw new ApplicationException("This shouldn't happen");
            });

            promise.Resolve();
        }

        [Fact]
        public void completed_handler_is_not_invoked_for_rejected_promise()
        {
            var promise = new Promise();

            promise.Done(() =>
            {
                throw new ApplicationException("This shouldn't happen");
            });

            promise.Reject(new ApplicationException("Rejection!"));
        }

        [Fact]
        public void chain_multiple_promises()
        {
            var promise = new Promise();
            var chainedPromise1 = new Promise();
            var chainedPromise2 = new Promise();

            var completed = 0;

            promise
                .ThenAll(() => LinqExts.FromItems(chainedPromise1, chainedPromise2).Cast<IPromise>())
                .Done(() => ++completed);

            Assert.Equal(0, completed);

            promise.Resolve();

            Assert.Equal(0, completed);

            chainedPromise1.Resolve();

            Assert.Equal(0, completed);

            chainedPromise2.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void chain_multiple_value_promises()
        {
            var promise = new Promise();
            var chainedPromise1 = new Promise<int>();
            var chainedPromise2 = new Promise<int>();
            var chainedResult1 = 10;
            var chainedResult2 = 15;

            var completed = 0;

            promise
                .ThenAll(() => LinqExts.FromItems(chainedPromise1, chainedPromise2).Cast<IPromise<int>>())
                .Done(result =>
                {
                    var items = result.ToArray();
                    Assert.Equal(2, items.Length);
                    Assert.Equal(chainedResult1, items[0]);
                    Assert.Equal(chainedResult2, items[1]);

                    ++completed;
                });

            Assert.Equal(0, completed);

            promise.Resolve();

            Assert.Equal(0, completed);

            chainedPromise1.Resolve(chainedResult1);

            Assert.Equal(0, completed);

            chainedPromise2.Resolve(chainedResult2);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void chain_multiple_value_promises_resolved_out_of_order()
        {
            var promise = new Promise();
            var chainedPromise1 = new Promise<int>();
            var chainedPromise2 = new Promise<int>();
            var chainedResult1 = 10;
            var chainedResult2 = 15;

            var completed = 0;

            promise
                .ThenAll(() => LinqExts.FromItems(chainedPromise1, chainedPromise2).Cast<IPromise<int>>())
                .Done(result =>
                {
                    var items = result.ToArray();
                    Assert.Equal(2, items.Length);
                    Assert.Equal(chainedResult1, items[0]);
                    Assert.Equal(chainedResult2, items[1]);

                    ++completed;
                });

            Assert.Equal(0, completed);

            promise.Resolve();

            Assert.Equal(0, completed);

            chainedPromise2.Resolve(chainedResult2);

            Assert.Equal(0, completed);

            chainedPromise1.Resolve(chainedResult1);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void combined_promise_is_resolved_when_children_are_resolved()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var all = Promise.All(LinqExts.FromItems<IPromise>(promise1, promise2));

            var completed = 0;

            all.Done(() => ++completed);

            promise1.Resolve();
            promise2.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void combined_promise_is_rejected_when_first_promise_is_rejected()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var all = Promise.All(LinqExts.FromItems<IPromise>(promise1, promise2));

            all.Done(() =>
            {
                throw new ApplicationException("Shouldn't happen");
            });

            var errors = 0;
            all.Catch(e =>
            {
                ++errors;
            });

            promise1.Reject(new ApplicationException("Error!"));
            promise2.Resolve();

            Assert.Equal(1, errors);
        }

        [Fact]
        public void combined_promise_is_rejected_when_second_promise_is_rejected()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var all = Promise.All(LinqExts.FromItems<IPromise>(promise1, promise2));

            all.Done(() =>
            {
                throw new ApplicationException("Shouldn't happen");
            });

            var errors = 0;
            all.Catch(e =>
            {
                ++errors;
            });

            promise1.Resolve();
            promise2.Reject(new ApplicationException("Error!"));

            Assert.Equal(1, errors);
        }

        [Fact]
        public void combined_promise_is_rejected_when_both_promises_are_rejected()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var all = Promise.All(LinqExts.FromItems<IPromise>(promise1, promise2));

            all.Done(() =>
            {
                throw new ApplicationException("Shouldn't happen");
            });

            var errors = 0;
            all.Catch(e =>
            {
                ++errors;
            });

            promise1.Reject(new ApplicationException("Error!"));
            promise2.Reject(new ApplicationException("Error!"));

            Assert.Equal(1, errors);
        }

        [Fact]
        public void combined_promise_is_resolved_if_there_are_no_children()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var all = Promise.All(LinqExts.Empty<IPromise>());

            var completed = 0;

            all.Done(() => ++completed);
        }

        [Fact]
        public void rejection_of_source_promise_rejects_resulting_promise()
        {
            var promise = new Promise();

            var ex = new Exception();
            var errors = 0;

            var transformedPromise = promise
                .ThenDo(() =>
                {
                    Assert.True(false, "This code shouldn't be executed");
                })
                .Catch(e =>
                {
                    Assert.Equal(ex, e);

                    ++errors;
                });

            promise.Reject(ex);

            Assert.Equal(1, errors);
        }

        [Fact]
        public void exception_thrown_during_transform_rejects_promise()
        {
            var promise = new Promise();

            var errors = 0;
            var ex = new Exception();

            var transformedPromise = promise
                .Then(() => 
                {
                    throw ex;
                })
                .Catch(e =>
                {
                    Assert.Equal(ex, e);

                    ++errors;
                });

            promise.Resolve();

            Assert.Equal(1, errors);
        }

        [Fact]
        public void can_chain_promise()
        {
            var promise = new Promise();
            var chainedPromise = new Promise();

            var completed = 0;

            promise
                .Then(() => chainedPromise)
                .Done(() => ++completed);

            promise.Resolve();
            chainedPromise.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_chain_promise_and_convert_to_promise_that_yields_a_value()
        {
            var promise = new Promise();
            var chainedPromise = new Promise<string>();
            var chainedPromiseValue = "some-value";

            var completed = 0;

            promise
                .Then(() => chainedPromise)
                .Done(v => 
                {
                    Assert.Equal(chainedPromiseValue, v);

                    ++completed;
                });

            promise.Resolve();
            chainedPromise.Resolve(chainedPromiseValue);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void exception_thrown_in_chain_rejects_resulting_promise()
        {
            var promise = new Promise();
            var chainedPromise = new Promise();

            var ex = new Exception();
            var errors = 0;

            var transformedPromise = promise
                .Then(() =>
                {
                    throw ex;
                })
                .Catch(e =>
                {
                    Assert.Equal(ex, e);

                    ++errors;
                });

            promise.Resolve();

            Assert.Equal(1, errors);
        }

        [Fact]
        public void rejection_of_source_promises_rejects_resulting_promise()
        {
            var promise = new Promise();
            var chainedPromise = new Promise();

            var ex = new Exception();
            var errors = 0;

            var transformedPromise = promise
                .Then(() => chainedPromise)
                .Catch(e =>
                {
                    Assert.Equal(ex, e);

                    ++errors;
                });

            promise.Reject(ex);

            Assert.Equal(1, errors);
        }

        [Fact]
        public void rejection_of_chained_promises_rejects_resulting_promise()
        {
            var promise = new Promise();
            var chainedPromise = new Promise();

            var ex = new Exception();
            var errors = 0;

            var transformedPromise = promise
                .Then(() => chainedPromise)
                .Catch(e =>
                {
                    Assert.Equal(ex, e);

                    ++errors;
                });

            promise.Resolve();
            chainedPromise.Reject(ex);

            Assert.Equal(1, errors);
        }

        [Fact]
        public void can_invoke_do_callback()
        {
            var promise = new Promise();
            var invoked = 0;
            promise.ThenDo(() => ++invoked);

            promise.Resolve();

            Assert.Equal(1, invoked);
        }

        [Fact]
        public void can_invoke_multiple_do_callbacks_in_order()
        {
            var promise = new Promise();
            var order = 0;
            promise
                .ThenDo(() => Assert.Equal(1, ++order))
                .ThenDo(() => Assert.Equal(2, ++order))
                .ThenDo(() => Assert.Equal(3, ++order));

            promise.Resolve();

            Assert.Equal(3, order);
        }

        [Fact]
        public void do_callback_is_not_invoked_when_promise_is_rejected()
        {
            var promise = new Promise();
            var invoked = 0;
            promise.ThenDo(() => ++invoked);

            promise.Reject(new ApplicationException());

            Assert.Equal(0, invoked);
        }

        [Fact]
        public void race_is_resolved_when_first_promise_is_resolved_first()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var completed = 0;

            Promise
                .Race(promise1, promise2)
                .Done(() => ++completed);

            promise1.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void race_is_resolved_when_second_promise_is_resolved_first()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            var completed = 0;

            Promise
                .Race(promise1, promise2)
                .Done(() => ++completed);

            promise2.Resolve();

            Assert.Equal(1, completed);
        }

        [Fact]
        public void race_is_rejected_when_first_promise_is_rejected_first()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            Exception ex = null;

            Promise
                .Race(promise1, promise2)
                .Catch(e => ex = e);

            var expected = new Exception();
            promise1.Reject(expected);

            Assert.Equal(expected, ex);
        }

        [Fact]
        public void race_is_rejected_when_second_promise_is_rejected_first()
        {
            var promise1 = new Promise();
            var promise2 = new Promise();

            Exception ex = null;

            Promise
                .Race(promise1, promise2)
                .Catch(e => ex = e);

            var expected = new Exception();
            promise2.Reject(expected);

            Assert.Equal(expected, ex);
        }

        [Fact]
        public void sequence_with_no_operations_is_directly_resolved()
        {
            var completed = 0;

            Promise
                .Sequence(new Func<IPromise>[0])
                .Done(() => ++completed);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void sequenced_is_not_resolved_when_operation_is_not_resolved()
        {
            var completed = 0;

            Promise
                .Sequence(() => new Promise())
                .Done(() => ++completed);

            Assert.Equal(0, completed);
        }

        [Fact]
        public void sequence_is_resolved_when_operation_is_resolved()
        {
            var completed = 0;

            Promise
                .Sequence(() => Promise.Resolved())
                .Done(() => ++completed);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void sequence_is_unresolved_when_some_operations_are_unresolved()
        {
            var completed = 0;

            Promise
                .Sequence(
                    () => Promise.Resolved(),
                    () => new Promise()
                )
                .Done(() => ++completed);

            Assert.Equal(0, completed);
        }

        [Fact]
        public void sequence_is_resolved_when_all_operations_are_resolved()
        {
            var completed = 0;

            Promise
                .Sequence(
                    () => Promise.Resolved(),
                    () => Promise.Resolved()
                )
                .Done(() => ++completed);

            Assert.Equal(1, completed);
        }

        [Fact]
        public void sequenced_operations_are_run_in_order_is_directly_resolved()
        {
            var order = 0;

            Promise
                .Sequence(
                    () =>
                    {
                        Assert.Equal(1, ++order);
                        return Promise.Resolved();
                    },
                    () =>
                    {
                        Assert.Equal(2, ++order);
                        return Promise.Resolved();
                    },
                    () =>
                    {
                        Assert.Equal(3, ++order);
                        return Promise.Resolved();
                    }
                );

            Assert.Equal(3, order);
        }

        [Fact]
        public void exception_thrown_in_sequence_rejects_the_promise()
        {
            var errored = 0;
            var completed = 0;
            var ex = new Exception();

            Promise
                .Sequence(() => 
                {
                    throw ex;
                })
                .Catch(e => {
                    Assert.Equal(ex, e);
                    ++errored;
                })
                .Done(() => ++completed);

            Assert.Equal(1, errored);
            Assert.Equal(0, completed);
        }

        [Fact]
        public void exception_thrown_in_sequence_stops_following_operations_from_being_invoked()
        {
            var completed = 0;

            Promise
                .Sequence(
                    () => 
                    {
                        ++completed;
                        return Promise.Resolved();
                    },
                    () =>
                    {
                        throw new Exception();
                    },
                    () =>
                    {
                        ++completed;
                        return Promise.Resolved();
                    }
                );

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_resolve_promise_via_resolver_function()
        {
            var promise = new Promise((resolve, reject) =>
            {
                resolve();
            });

            var completed = 0;
            promise.Done(() =>
            {
                ++completed;
            });

            Assert.Equal(1, completed);
        }

        [Fact]
        public void can_reject_promise_via_resolver_function()
        {
            var ex = new Exception();
            var promise = new Promise((resolve, reject) =>
            {
                reject(ex);
            });

            var completed = 0;
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed;
            });

            Assert.Equal(1, completed);
        }

        [Fact]
        public void exception_thrown_during_resolver_rejects_proimse()
        {
            var ex = new Exception();
            var promise = new Promise((resolve, reject) =>
            {
                throw ex;
            });

            var completed = 0;
            promise.Catch(e =>
            {
                Assert.Equal(ex, e);
                ++completed;
            });

            Assert.Equal(1, completed);
        }
    }
}
